using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		
    }

    // Update is called once per frame
    void Update()
    {
		
	}

	private void FixedUpdate()
	{
		rb.MovePosition(transform.position + transform.forward * Time.deltaTime * 10);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.GetComponent<ennemyBasic>() != null)
		{
			collision.collider.GetComponent<ennemyBasic>().dealDamage(15);
		}

		Destroy(gameObject);
	}
}
