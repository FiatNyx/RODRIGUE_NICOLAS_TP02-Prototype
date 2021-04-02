using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class player : MonoBehaviour
{
	Camera mainCam;
	NavMeshAgent navMeshAgent;
	int moveSelected = 0;
	public ParticleSystem particles;
	Animator animationJoueur;
	bool isRotating;

	int vie = 30;
	public int vieMax = 30;
	Vector3 rotationTarget;
	public float RotationSpeed;

	//values for internal use
	private Quaternion _lookRotation;
	private Vector3 _direction;
	Vector3 cible;

	public GameObject fireball;
	public GameObject marqueur1;
	public GameObject marqueur2;
	public Transform projectileStartPoint;
	public bool isAttacking = false;
	Vector3 moveDirection;
	Rigidbody rb;
	public GameObject cercleLentPrefab;

	public Transform cameraPosition;

	bool isSlowed = false;
	bool isPoisoned = false;
	float timerPoison = 0;

	AudioSource audioSource;
	public AudioClip audioDamage;
	public AudioClip audioEclair;
	public AudioClip audioZoom;
	public AudioClip audioBoom;
	
	// Start is called before the first frame update
	void Start()
	{
		mainCam = Camera.main;
		UI_Manager.singleton.changeVieText(vieMax, vie);
	}

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animationJoueur = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		vie = vieMax;
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
    {

		if(vie <= 0)
		{
			Destroy(gameObject);
		}
		cameraPosition.position = transform.position;
		


		animationJoueur.SetBool("Idle", false);

		if (GameManager.singleton.getPlayerTurn() && isAttacking == false)
		{
			

            if (Input.GetMouseButton(2))
            {
				RotateCamera();
            }
            else
            {
				Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;

				if (Physics.Raycast(camRay, out hit))
				{
					//Demander au pnj de s'y rendre
					Vector3 lookDirection = hit.point;
					lookDirection.y = transform.position.y;

					Vector3 relativePos = lookDirection - transform.position;

					// the second argument, upwards, defaults to Vector3.up
					Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
					transform.rotation = rotation;
				}
			}
			

            if (Input.GetMouseButtonDown(1))
            {
				resetAttackSelected();
			}




			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				effacerMarqueurs();
				marqueur1.SetActive(true);
				marqueur1.transform.position = transform.position;
				moveSelected = 1;
				UI_Manager.singleton.changeSelectedMove(1);
			
			}else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				effacerMarqueurs();
				Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;

				if (Physics.Raycast(camRay, out hit))
				{
					marqueur2.SetActive(true);
					marqueur2.transform.position = hit.point;
					moveSelected = 2;
				}
				UI_Manager.singleton.changeSelectedMove(2);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				moveSelected = 3;
				UI_Manager.singleton.changeSelectedMove(3);
				
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				moveSelected = 4;
				UI_Manager.singleton.changeSelectedMove(4);
				
			}




			if(moveSelected == 0)
            {
				float inputVertical = Input.GetAxis("Vertical");

				float inputHorizontal = Input.GetAxis("Horizontal");

				animationJoueur.SetFloat("horizontal", inputHorizontal);
				animationJoueur.SetFloat("vertical", inputVertical);

				if (isPoisoned && (Mathf.Abs(inputHorizontal) > 0 || Mathf.Abs(inputVertical) > 0))
				{
					timerPoison += Time.deltaTime;

					if(timerPoison > 0.5)
					{
						damage(3);
						timerPoison = 0;
					}
				}

			
				moveDirection = transform.forward * inputVertical + transform.right * inputHorizontal;
				
				
            }
            else
            {
				moveDirection = Vector3.zero;
				animationJoueur.SetBool("Idle", true);

				if(moveSelected == 2)
				{
					Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

					RaycastHit hit;

					if (Physics.Raycast(camRay, out hit))
					{
						
						marqueur2.transform.position = hit.point;
					
					}
				}


				if (Input.GetMouseButtonDown(0))
				{
					if (moveSelected == 1 && GameManager.singleton.getTimerJoueur() > 2)
					{
						isAttacking = true;
						StartCoroutine(BouleDeFeu());
						
					}
					else if(moveSelected == 2 && GameManager.singleton.getTimerJoueur() > 2)
					{
						Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

						RaycastHit hit;

						if (Physics.Raycast(camRay, out hit))
						{
							GameManager.singleton.StartAttack(2);
							GameObject cercleLent = Instantiate(cercleLentPrefab, hit.point, transform.rotation);
							//Particle system ici
							GameManager.singleton.FinishAttack();
						}
					}
					else if(moveSelected == 3 && GameManager.singleton.getTimerJoueur() > 2)
					{
						audioSource.PlayOneShot(audioEclair);
					}
					else if(moveSelected == 4 && GameManager.singleton.getTimerJoueur() > 2)
					{
						Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

						RaycastHit hit;

						if (Physics.Raycast(camRay, out hit))
						{
							GameManager.singleton.StartAttack(2);
							transform.position = hit.point;
							//Particle system ici
							GameManager.singleton.FinishAttack();
							audioSource.PlayOneShot(audioZoom);
						}

					
					}
				}
			}
        }
        else
        {
			resetAttackSelected();
			moveDirection = Vector3.zero;
			animationJoueur.SetBool("Idle", true);
        }
	}

	void resetAttackSelected()
	{
		moveSelected = 0;
		effacerMarqueurs();
		UI_Manager.singleton.changeSelectedMove(0);
	}


	public void damage(int damage)
	{
		vie -= damage;
		UI_Manager.singleton.changeVieText(vieMax, vie);
		audioSource.PlayOneShot(audioDamage);
	}
	private void effacerMarqueurs()
	{
		marqueur1.SetActive(false);
		marqueur2.SetActive(false);
	}
    private void FixedUpdate()
    {
		float speed = 5f;
		if (isSlowed)
		{
			speed /= 2f;
		}
		
		rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.fixedDeltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "LentPoison")
		{
			isSlowed = true;
			isPoisoned = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "LentPoison")
		{
			isSlowed = false;
			isPoisoned = false;
		}
	}

	void RotateCamera()
	{
		Vector3 rotCamera = cameraPosition.rotation.eulerAngles;

		// Le player tourne en fonction de la position de la souris (y seulement)
		rotCamera.y += Input.GetAxis("Mouse X") * 10;

		// Appliquer la rotation rotPlayer dans la rotation du Transform (Quaternion)
		cameraPosition.rotation = Quaternion.Euler(rotCamera);
	}

	IEnumerator BouleDeFeu()
	{
		GameManager.singleton.StartAttack(2);
		GameObject bouleDeFeu = Instantiate(fireball, projectileStartPoint.position, projectileStartPoint.rotation);
		bouleDeFeu.GetComponent<Fireball>().joueur = this;

		float timerMove = 0;
		while (isAttacking && timerMove < 2)
		{
			timerMove += Time.deltaTime;
			yield return null;
		}

		isAttacking = false;
		GameManager.singleton.FinishAttack();
	}
}
