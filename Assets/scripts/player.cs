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

	// Start is called before the first frame update
	void Start()
	{
		mainCam = Camera.main;

	}

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update()
    {
		if (GameManager.singleton.getPlayerTurn())
		{

		
			if (Input.GetMouseButtonDown(0))
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

					}
				}else if(moveSelected == 1)
				{
					Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

					RaycastHit hit;
					//Y a-t-il eu un impact
					if (Physics.Raycast(camRay, out hit))
					{
						//Demander au pnj de s'y rendre
						particles.transform.position = hit.point;
						particles.Play();

					}
				}

			
			
			}

			if(Input.GetMouseButtonDown(1))
			{
				moveSelected = 0;
			}

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				moveSelected = 1;
			}
		

			if(GameManager.singleton.getTimerJoueur() <= 0f)
			{
				navMeshAgent.isStopped = true;
				navMeshAgent.ResetPath();
				navMeshAgent.isStopped = false;
			}

		}
	}
}
