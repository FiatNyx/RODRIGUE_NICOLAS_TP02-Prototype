using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	float timerDestruction = 0;
	public player joueur;

	// Start is called before the first frame update
	void Start()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, 10);
		foreach (Collider item in colliders)
		{
			Rigidbody rb = item.GetComponent<Rigidbody>();

			if (rb != null)
			{
				//Appliquer une vélocité
				rb.AddExplosionForce(10, transform.position, 10, 1, ForceMode.Impulse);
			}
		}
	}
    // Update is called once per frame
    void Update()
    {
		timerDestruction += Time.deltaTime;
		if (timerDestruction > 0.5)
		{

			joueur.isAttacking = false;
			Destroy(gameObject);
		}
	}
}
