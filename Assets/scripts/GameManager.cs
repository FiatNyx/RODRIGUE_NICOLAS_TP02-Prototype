using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public static GameManager singleton;

	float timerJoueur = 0f;
	bool isPlayerTurn = true;
	public float tempsTourJoueur = 5f;

	private void Awake()
	{
		if (singleton != null)
		{
			Debug.LogError("Détection de multiples instances du GameManager.");
			return;
		}

		// Assignation du singleton
		singleton = this;
	}

	public void changeTurn()
	{
		if(isPlayerTurn == true)
		{
			isPlayerTurn = false;
		}
		else
		{
			timerJoueur = 0f;
			isPlayerTurn = true;
		}
	}

	public bool getPlayerTurn()
	{
		return isPlayerTurn;
	}

    public void Update()
    {
        if (isPlayerTurn)
        {
			timerJoueur += Time.deltaTime;
        }
    }

	public float getTimerJoueur()
    {
		return timerJoueur;
    }
}
