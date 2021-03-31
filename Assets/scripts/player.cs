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


	Vector3 rotationTarget;
	public float RotationSpeed;

	//values for internal use
	private Quaternion _lookRotation;
	private Vector3 _direction;
	Vector3 cible;

	public GameObject fireball;
	public GameObject marqueur1;
	public Transform projectileStartPoint;

	Vector3 moveDirection;
	Rigidbody rb;

	public Transform cameraPosition;


	// Start is called before the first frame update
	void Start()
	{
		mainCam = Camera.main;

	}

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animationJoueur = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
    {
		cameraPosition.position = transform.position;
		


		animationJoueur.SetBool("Idle", false);

		if (GameManager.singleton.getPlayerTurn())
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
				moveSelected = 0;
				marqueur1.SetActive(false);
			}

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				marqueur1.SetActive(true);
				marqueur1.transform.position = transform.position;
				moveSelected = 1;
			
			}else if (Input.GetKeyDown(KeyCode.Alpha2))
			{

			}else if (Input.GetKeyDown(KeyCode.Alpha3))
			{

			}else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				moveSelected = 4;
			}
			if(moveSelected == 0)
            {
				float inputVertical = Input.GetAxis("Vertical");

				float inputHorizontal = Input.GetAxis("Horizontal");

				animationJoueur.SetFloat("horizontal", inputHorizontal);
				animationJoueur.SetFloat("vertical", inputVertical);

				moveDirection = transform.forward * inputVertical + transform.right * inputHorizontal;
            }
            else
            {
				moveDirection = Vector3.zero;
				animationJoueur.SetBool("Idle", true);


				if (Input.GetMouseButtonDown(0))
				{
					if (moveSelected == 1 && GameManager.singleton.getTimerJoueur() > 2)
					{
						GameManager.singleton.StartAttack(2);
						Instantiate(fireball, projectileStartPoint.position, projectileStartPoint.rotation);
						GameManager.singleton.FinishAttack();
					}
					else if(moveSelected == 2 && GameManager.singleton.getTimerJoueur() > 2)
					{

					}
					else if(moveSelected == 3 && GameManager.singleton.getTimerJoueur() > 2)
					{

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
						}
					}
				}
			}
            
			


            /*
			if (Input.GetMouseButtonDown(0) && isRotating == false)
			{
				if (moveSelected == 0)
				{
					//Créer un rayon entre la caméra et le curseur.
					Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

					RaycastHit hit;
					//Y a-t-il eu un impact
					if (Physics.Raycast(camRay, out hit))
					{
						//Demander au pnj de s'y rendre
						navMeshAgent.SetDestination(hit.point);
						animationJoueur.SetBool("Running", true);
					}
				}
                else if(navMeshAgent.remainingDistance > 0.2f || navMeshAgent.pathPending)
                {
					navMeshAgent.isStopped = true;
					navMeshAgent.ResetPath();
					navMeshAgent.isStopped = false;
				}
				if(navMeshAgent.remainingDistance < 0.2f && navMeshAgent.pathPending == false)
                {
					if (moveSelected == 1 && GameManager.singleton.getTimerJoueur() > 2)
					{
						GameManager.singleton.StartAttack(2);
						Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

						RaycastHit hit;
						//Y a-t-il eu un impact
						if (Physics.Raycast(camRay, out hit))
						{
							
							
							cible = hit.point;
							rotationTarget = cible;
							rotationTarget.y = transform.position.y;

							
							isRotating = true;

							//find the vector pointing from our position to the target
							_direction = (rotationTarget - transform.position).normalized;


							//create the rotation we need to be in to look at the target
							_lookRotation = Quaternion.LookRotation(_direction);

							print(_lookRotation.y - transform.rotation.y);
							if (_lookRotation.y - transform.rotation.y > 0)
							{
								animationJoueur.SetTrigger("TurnLeft");
								
							}
							else if (_lookRotation.y - transform.rotation.y < 0)
							{
							
								animationJoueur.SetTrigger("TurnRight");
							}

							
						}

						
						moveSelected = 0;
					}
                }


				
			
			}

			if (isRotating)
			{
				
				navMeshAgent.isStopped = true;
				
				//find the vector pointing from our position to the target
				_direction = (rotationTarget - transform.position).normalized;

				
				//create the rotation we need to be in to look at the target
				_lookRotation = Quaternion.LookRotation(_direction);

				
				//rotate us over time according to speed until we are in the required rotation
				transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.deltaTime * 100);

				
				if (transform.rotation == _lookRotation)
				{
					//particles.transform.position = cible;
					//particles.Play();
					Instantiate(fireball, projectileStartPoint.position, projectileStartPoint.rotation);
					GameManager.singleton.FinishAttack();
					isRotating = false;
					navMeshAgent.isStopped = false;
					animationJoueur.SetBool("TurnRight", false);
				}
			}
			if (Input.GetMouseButtonDown(1))
			{
                if (animationJoueur.GetBool("Running"))
                {
					navMeshAgent.isStopped = true;
					navMeshAgent.ResetPath();
					navMeshAgent.isStopped = false;
					animationJoueur.SetBool("Running", false);
				}
				moveSelected = 0;
			}


			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				marqueur1.SetActive(true);
				marqueur1.transform.position = transform.position;
				moveSelected = 1;
				if (animationJoueur.GetBool("Running"))
				{
					navMeshAgent.isStopped = true;
					navMeshAgent.ResetPath();
					navMeshAgent.isStopped = false;
					animationJoueur.SetBool("Running", false);
				}
			}

			if(animationJoueur.GetBool("Running") && navMeshAgent.remainingDistance < 0.2f && navMeshAgent.pathPending == false)
            {
				animationJoueur.SetBool("Running", false);
            }

			if(GameManager.singleton.getTimerJoueur() <= 0.1f)
			{
				navMeshAgent.isStopped = true;
				navMeshAgent.ResetPath();
				navMeshAgent.isStopped = false;
				animationJoueur.SetBool("Running", false);
				moveSelected = 0;
			}

			if(moveSelected == 0)
			{
				if(marqueur1.activeSelf == true)
				{
					marqueur1.SetActive(false);
				}
			}
			if(moveSelected == 1)
			{
				Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;
				//Y a-t-il eu un impact
				if (Physics.Raycast(camRay, out hit))
				{
					Vector3 cible = hit.point;
					cible.y = marqueur1.transform.position.y;
					marqueur1.transform.LookAt(cible);
				}
			}
			*/


        }
        else
        {
			moveDirection = Vector3.zero;
			animationJoueur.SetBool("Idle", true);
        }
	}


    private void FixedUpdate()
    {
		rb.MovePosition(rb.position + moveDirection.normalized * 5 * Time.fixedDeltaTime);
		

		

		//transform.LookAt(lookDirection);

		//transform.Rotate(rotationDirection);
	}


	void RotateCamera()
	{
		Vector3 rotCamera = cameraPosition.rotation.eulerAngles;

		// Le player tourne en fonction de la position de la souris (y seulement)
		rotCamera.y += Input.GetAxis("Mouse X") * 10;

		// Appliquer la rotation rotPlayer dans la rotation du Transform (Quaternion)
		cameraPosition.rotation = Quaternion.Euler(rotCamera);
	}
}
