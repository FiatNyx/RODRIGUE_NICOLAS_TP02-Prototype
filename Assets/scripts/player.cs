using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class player : MonoBehaviour
{
	Camera mainCam;
	NavMeshAgent navMeshAgent;
	bool clicked = false;
	// Start is called before the first frame update
	void Start()
	{
		mainCam = Camera.main;
		clicked = false;
	}

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetMouseButtonDown(0) && GameManager.singleton.getPlayerTurn() && clicked == false)
		{
			//Créer un rayon entre la caméra et le curseur.
			Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;
			//Y a-t-il eu un impact
			if (Physics.Raycast(camRay, out hit))
			{
				//Demander au pnj de s'y rendre
				navMeshAgent.SetDestination(hit.point);
				clicked = true;
			}
		}

		if(clicked == true && navMeshAgent.pathPending == false && navMeshAgent.remainingDistance < 0.5f)
		{
			GameManager.singleton.changeTurn();
			clicked = false;
		}
	}
}
