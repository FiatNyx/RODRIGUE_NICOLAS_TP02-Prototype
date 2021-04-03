using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportParticles : MonoBehaviour
{
	bool isSpinning;
	float timerSpin = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (isSpinning)
		{
			transform.Rotate(Vector3.forward * 3);
			timerSpin += Time.deltaTime;
			if(timerSpin >= 0.75f)
			{
				isSpinning = false;
			}
		}   
    }

	public void Spin()
	{
		isSpinning = true;
		timerSpin = 0f;
	}
}
