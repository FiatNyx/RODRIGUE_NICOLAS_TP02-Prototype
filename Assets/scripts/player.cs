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

	// Start is called before the first frame update
	void Start()
	{
		mainCam = Camera.main;

	}

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animationJoueur = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
    {
		if (GameManager.singleton.getPlayerTurn())
		{
			
		
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
						}

						
						moveSelected = 0;
					}
                }


				
			
			}

			if (isRotating)
			{
				animationJoueur.SetBool("Turn", true);
				navMeshAgent.isStopped = true;
				print("Is rotating");
				//find the vector pointing from our position to the target
				_direction = (rotationTarget - transform.position).normalized;

				print(_direction);
				//create the rotation we need to be in to look at the target
				_lookRotation = Quaternion.LookRotation(_direction);

				print(_lookRotation);

				//rotate us over time according to speed until we are in the required rotation
				transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.deltaTime * 100);

				print(transform.rotation);
				if (transform.rotation == _lookRotation)
				{
					particles.transform.position = cible;
					particles.Play();
					GameManager.singleton.FinishAttack();
					isRotating = false;
					navMeshAgent.isStopped = false;

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
				moveSelected = 1;
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
			}

		}
	}
}
