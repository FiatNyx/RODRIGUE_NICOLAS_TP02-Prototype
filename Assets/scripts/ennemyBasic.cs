﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemyBasic : MonoBehaviour
{
	NavMeshAgent navMeshAgent;
	bool isMoving = false;
	float timerMove = 0f;
	public GameObject player;

	int health = 15;
	private Animator animation;
	// Start is called before the first frame update
	void Start()
    {
        
    }

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		timerMove = 0f;
		animation = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
    {
		if (health <= 0)
		{
			Destroy(gameObject);
			return;
		}

		if (isMoving == false && GameManager.singleton.getPlayerTurn() == false)
		{
			timerMove = 0f;
			StartCoroutine(Mouvement());
		}

		
	}

	IEnumerator Mouvement()
	{
		animation.SetBool("Running", true);
		isMoving = true;
		navMeshAgent.isStopped = false;
		

		//Me déplacer vers la destination
		navMeshAgent.SetDestination(player.transform.position);
		

		//Tant que je ne suis pas rendu à destination, je ne fait rien d'autre
		while (navMeshAgent.pathPending || (navMeshAgent.remainingDistance > 0.5f && timerMove < 3f))
		{
		
			timerMove += Time.deltaTime;

			yield return null;
		}
		
		if (navMeshAgent.remainingDistance > 0.5f)
		{
			navMeshAgent.isStopped = true;
			
		}

		animation.SetBool("Running", false);
		GameManager.singleton.changeTurn();
		isMoving = false;
	}


	public void dealDamage(int damage)
	{
		health -= damage;
	}
}
