using System.Collections;
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
	public GameObject sliderVie;

	AudioSource audioSource;
	public AudioClip audioOuch;
	public AudioClip audioAttack;
	public AudioClip audioMortEnnemy;
	public Collider ennemyCollider;
	
	Rigidbody[] ragdollRBs;
	Collider[] ragdollColliders;

	// Start is called before the first frame update
	void Start()
    {
		ragdollRBs = GetComponentsInChildren<Rigidbody>();
		ragdollColliders = GetComponentsInChildren<Collider>();
		audioSource = GetComponent<AudioSource>();
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

				if (timerPoison > 0.8)
				{
					dealDamage(8);
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
			transform.LookAt(player.transform.position);
			animationEnnemy.SetBool("Running", false);
			GameManager.singleton.StartAttack(0);
			while(timerAttack < 1f)
			{
				timerAttack += Time.deltaTime;
				yield return null;
			}

			audioSource.PlayOneShot(audioAttack);
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
		sliderVie.GetComponent<Slider>().value = (float)health / (float)maxHealth * 100f;

		audioSource.PlayOneShot(audioOuch);
		if (health <= 0)
		{
			
			StopAllCoroutines();
			audioSource.Stop();
			audioSource.PlayOneShot(audioMortEnnemy);

			animationEnnemy.enabled = false;
			navMeshAgent.enabled = false;
			ennemyCollider.enabled = false;
			GameManager.singleton.killEnnemy(transform);
			foreach (Collider rbcollider in ragdollColliders)
			{
				rbcollider.enabled = true;
			}

			foreach (Rigidbody rb in ragdollRBs)
			{
				rb.isKinematic = false;
			}
			sliderVie.SetActive(false);
			this.enabled = false;

		}
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


