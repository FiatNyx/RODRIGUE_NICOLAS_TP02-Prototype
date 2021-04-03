using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eclair : MonoBehaviour
{

	Transform[] miniEclairs;
	float timerDestruction = 0f;
	public player joueur;
	
	// Start is called before the first frame update
	void Start()
    {
		miniEclairs = GetComponentsInChildren<Transform>();
    }


	private void FixedUpdate()
	{
		transform.localScale += Vector3.forward * 10 * Time.fixedDeltaTime;
		
		timerDestruction += Time.deltaTime;
		if (timerDestruction > 0.5f)
		{
			joueur.isAttacking = false;
			Destroy(gameObject);
		}
	}

	
}
