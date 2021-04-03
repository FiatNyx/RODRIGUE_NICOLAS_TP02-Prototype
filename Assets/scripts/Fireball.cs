using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	Rigidbody rb;
	public player joueur;

	public GameObject explosion;
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
			GameObject explosionInstance = Instantiate(explosion, transform.position, transform.rotation);
			explosionInstance.GetComponent<Explosion>().joueur = joueur;
			
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.GetComponent<ennemyBasic>() != null)
		{
			collision.collider.GetComponent<ennemyBasic>().dealDamage(10);
			
		}
		GameObject explosionInstance = Instantiate(explosion, transform.position, transform.rotation);
		
		explosionInstance.GetComponent<Explosion>().joueur = joueur;
		Destroy(gameObject);
	}
}
