using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	Rigidbody rb;
	public player joueur;
	

	float timerDestruction = 0;
    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		timerDestruction = 0;
		
    }

	private void FixedUpdate()
	{
		rb.MovePosition(transform.position + transform.forward * Time.deltaTime * 10);
		timerDestruction += Time.deltaTime;
		if(timerDestruction > 2)
		{
			joueur.isAttacking = false;
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.GetComponent<ennemyBasic>() != null)
		{
			collision.collider.GetComponent<ennemyBasic>().dealDamage(15);
			
		}

		joueur.isAttacking = false;
		Destroy(gameObject);
	}
}
