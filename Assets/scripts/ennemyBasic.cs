﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ennemyBasic : MonoBehaviour
{
	NavMeshAgent navMeshAgent;
	bool isMoving = false;
	
	float timerMove = 0f;
	public GameObject player;

	public int maxHealth = 50;
	int health = 50;
	private Animator animationEnnemy;


	bool isPoisoned;
	float timerPoison = 0f;
	float speed = 0f;
	public bool isThisEnnemyTurn;
	public Slider sliderVie;


	// Start is called before the first frame update
	void Start()
    {
        
    }

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		timerMove = 0f;
		animationEnnemy = GetComponent<Animator>();
		speed = navMeshAgent.speed;
		health = maxHealth;
	}

	// Update is called once per frame
	void Update()
    {
		if (health <= 0)
		{
			GameManager.singleton.killEnnemy(transform);
			Destroy(gameObject);
			return;
		}

		if (isMoving == false && GameManager.singleton.getPlayerTurn() == false && isThisEnnemyTurn)
		{
			timerMove = 0f;
			StartCoroutine(Mouvement());
		}

		
	}

	IEnumerator Mouvement() //Changer mouvement pour les autres types d'ennemis, genre le mettre dans un autre component
	{
		animationEnnemy.SetBool("Running", true);
		isMoving = true;
		navMeshAgent.isStopped = false;
		

		//Me déplacer vers la destination
		navMeshAgent.SetDestination(player.transform.position);
		

		//Tant que je ne suis pas rendu à destination, je ne fait rien d'autre
		while (navMeshAgent.pathPending || (navMeshAgent.remainingDistance > 3f && timerMove < 3f))
		{
			Debug.Log(navMeshAgent.remainingDistance);
			timerMove += Time.deltaTime;

			if (isPoisoned)
			{
				timerPoison += Time.deltaTime;

				if (timerPoison > 0.3)
				{
					dealDamage(3);
					timerPoison = 0;
				}
			}
			yield return null;
		}

		

		
		float timerAttack = 0f;
		if (navMeshAgent.remainingDistance <= 3f)
		{
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
			animationEnnemy.SetBool("Running", false);
			GameManager.singleton.StartAttack(0);
			while(timerAttack < 1f)
			{
				timerAttack += Time.deltaTime;
				yield return null;
			}

			animationEnnemy.SetTrigger("Attack");
			player.GetComponent<Animator>().SetTrigger("Hurt"); //Timer animation joueur avec l'attaque
			while (timerAttack < 3f)
			{
				
				timerAttack += Time.deltaTime;
				yield return null;
			}
			player.GetComponent<player>().damage(10);
			
			GameManager.singleton.FinishAttack();
			
		}

		navMeshAgent.isStopped = true;
		navMeshAgent.ResetPath();
		animationEnnemy.SetBool("Running", false);


		GameManager.singleton.changeTurn();
		isMoving = false;
	}

	/* Vector3 toPlayer = player.transform.position - transform.position;
 if (Vector3.Distance(player.transform.position - transform.position) < 3) {
   Vector3 targetPosition = toPlayer.normalized * -3f;
   navMeshAgent.destination = targetPosition;
   navMeshAgent.Resume();
 }
 */
	public void dealDamage(int damage)
	{
		health -= damage;
		sliderVie.value = (float)health / (float)maxHealth * 100f;
		print(health / maxHealth);
	}

	private void OnTriggerEnter(Collider other)
	{
		
		if (other.tag == "LentPoison")
		{
			
			navMeshAgent.speed = speed / 2;
			isPoisoned = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "LentPoison")
		{
			navMeshAgent.speed = speed;
			isPoisoned = false;
		}
	}

}


