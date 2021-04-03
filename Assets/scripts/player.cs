using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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


	//values for internal use
	private Quaternion _lookRotation;
	private Vector3 _direction;
	Vector3 cible;

	public GameObject fireball;
	public GameObject marqueur1;
	public GameObject marqueur2;
	public GameObject marqueur3;
	public GameObject marqueur4;

	public Transform projectileStartPoint;
	public bool isAttacking = false;
	Vector3 moveDirection;
	Rigidbody rb;
	public GameObject cercleLentPrefab;

	public Transform cameraPosition;

	bool isSlowed = false;
	bool isPoisoned = false;
	float timerPoison = 0;
	public GameObject eclair;
	AudioSource audioSource;
	public AudioClip audioDamage;
	public AudioClip audioEclair;
	public AudioClip audioZoom;
	public AudioClip audioBoom;
	
	public ParticleSystem teleportParticles;
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
					
				}
				moveSelected = 2;
				UI_Manager.singleton.changeSelectedMove(2);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				effacerMarqueurs();
				marqueur3.SetActive(true);
				marqueur3.transform.position = transform.position;
				moveSelected = 3;
				UI_Manager.singleton.changeSelectedMove(3);
				
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				effacerMarqueurs();
				Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;

				if (Physics.Raycast(camRay, out hit))
				{
					marqueur4.SetActive(true);
					marqueur4.transform.position = hit.point;
					
				}

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


				Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;

				if (Physics.Raycast(camRay, out hit))
				{
					projectileStartPoint.LookAt(hit.point);
					marqueur1.transform.LookAt(hit.point);
					marqueur3.transform.LookAt(hit.point);
					if (moveSelected == 2)
					{
						marqueur2.transform.position = hit.point;
					}
					else if(moveSelected == 4)
					{
						marqueur4.transform.position = hit.point;
					}
				}

				


				if (Input.GetMouseButtonDown(0))
				{
					if (moveSelected == 1 && GameManager.singleton.getTimerJoueur() > 2)
					{
						isAttacking = true;
						animationJoueur.SetTrigger("FireballAttack");
						StartCoroutine(BouleDeFeu());
						
					}
					else if(moveSelected == 2 && GameManager.singleton.getTimerJoueur() > 4)
					{
						

						if (Physics.Raycast(camRay, out hit))
						{
							GameManager.singleton.StartAttack(4);
							GameObject cercleLent = Instantiate(cercleLentPrefab, hit.point, transform.rotation);
							
							GameManager.singleton.FinishAttack();
						}
					}
					else if(moveSelected == 3 && GameManager.singleton.getTimerJoueur() > 3)
					{
						isAttacking = true;
						animationJoueur.SetTrigger("LightningAttack");
						StartCoroutine(Eclair());
						audioSource.PlayOneShot(audioEclair);
					}
					else if(moveSelected == 4 && GameManager.singleton.getTimerJoueur() > 6)
					{
					
						if (Physics.Raycast(camRay, out hit))
						{
							GameManager.singleton.StartAttack(6);
							transform.position = hit.point;
							teleportParticles.Play();
							teleportParticles.GetComponent<TeleportParticles>().Spin();
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

			if (Input.GetMouseButton(2))
			{
				RotateCamera();
			}
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

		if (vie <= 0)
		{
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.name);
		}
	}

	private void effacerMarqueurs()
	{
		marqueur1.SetActive(false);
		marqueur2.SetActive(false);
		marqueur3.SetActive(false);
		marqueur4.SetActive(false);
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

		float timerMove = 0;
		while (isAttacking && timerMove < 1.5f)
		{
			timerMove += Time.deltaTime;
			yield return null;
		}

		
		
		GameObject bouleDeFeu = Instantiate(fireball, projectileStartPoint.position, projectileStartPoint.rotation);
		bouleDeFeu.GetComponent<Fireball>().joueur = this;

		timerMove = 0;
		while (isAttacking && timerMove < 2)
		{
			timerMove += Time.deltaTime;
			yield return null;
		}

		isAttacking = false;
		GameManager.singleton.FinishAttack();
	}


	IEnumerator Eclair()
	{

		float timerMove = 0;
		GameManager.singleton.StartAttack(3);

		while (isAttacking && timerMove < 1)
		{
			timerMove += Time.deltaTime;
			yield return null;
		}
		GameObject eclairInstance = Instantiate(eclair, projectileStartPoint.position, projectileStartPoint.rotation);
		eclairInstance.GetComponent<Eclair>().joueur = this;

		timerMove = 0;
		while (isAttacking && timerMove < 2)
		{
			timerMove += Time.deltaTime;
			yield return null;
		}

		isAttacking = false;
		GameManager.singleton.FinishAttack();
	}
}
