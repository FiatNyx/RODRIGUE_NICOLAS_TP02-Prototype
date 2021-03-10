using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemyBasic : MonoBehaviour
{
	NavMeshAgent navMeshAgent;
	bool isMoving = false;
	float timerMove = 0f;
	public GameObject player;
	// Start is called before the first frame update
	void Start()
    {
        
    }

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		timerMove = 0f;
		StartCoroutine(Mouvement());
	}

	// Update is called once per frame
	void Update()
    {
		
	}

	IEnumerator Mouvement()
	{
		Debug.Log("Se déplace");
		isMoving = true;

		navMeshAgent.speed = 3;

		//Me déplacer vers la destination
		navMeshAgent.SetDestination(player.transform.position);
		

		//Tant que je ne suis pas rendu à destination, je ne fait rien d'autre
		while (navMeshAgent.pathPending || (navMeshAgent.remainingDistance > 0.5f && timerMove < 3f))
		{
			Debug.Log("Salut");
			Debug.Log(timerMove);
			timerMove += Time.deltaTime;
			yield return null;
		}

		print("Finis");
		//Rendu à destination, je prend une pause (Bien méritée)
		if (navMeshAgent.remainingDistance > 0.5f)
		{
			navMeshAgent.isStopped = true;
		}

		//Je démarre une nouvelle patrouille
		isMoving = false;
	}
}
