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

    private void Start()
    {
		timerJoueur = tempsTourJoueur;
    }


    public void changeTurn()
	{
		if(isPlayerTurn == true)
		{
			isPlayerTurn = false;
			UI_Manager.singleton.changeTurnText(false);
		}
		else
		{
			timerJoueur = tempsTourJoueur;
			isPlayerTurn = true;
			UI_Manager.singleton.changeTurnText(true);
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
			timerJoueur -= Time.deltaTime;

			if(timerJoueur <= 0f)
            {
				changeTurn();
            }
        }
    }

	public float getTimerJoueur()
    {
		return timerJoueur;
    }
}
