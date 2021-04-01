﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public static GameManager singleton;

	float timerJoueur = 0f;
	bool isPlayerTurn = true;
	public float tempsTourJoueur = 10f;
	bool isTimerStopped = false;
	float timerEnnemy = 0f;
	float tempsTourEnnemy = 5f;

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
			timerEnnemy = tempsTourEnnemy;
			isTimerStopped = false;
		}
		else
		{
			timerJoueur = tempsTourJoueur;
			isPlayerTurn = true;
			UI_Manager.singleton.changeTurnText(true);
			isTimerStopped = false;
		}
	}

	public bool getPlayerTurn()
	{
		return isPlayerTurn;
	}

	public void StartAttack(float cost)
	{
		isTimerStopped = true;
		timerJoueur -= cost;
	}

	public void FinishAttack()
	{
		isTimerStopped = false;
	}
	
    public void Update()
    {
        if (isPlayerTurn && isTimerStopped == false)
        {
			timerJoueur -= Time.deltaTime;

			UI_Manager.singleton.UpdateTimer(timerJoueur);
			if (timerJoueur <= 0f)
            {
				changeTurn();
            }
        }else if(isPlayerTurn == false && isTimerStopped == false)
        {
			timerEnnemy -= Time.deltaTime;

			UI_Manager.singleton.UpdateTimer(timerEnnemy);
			if (timerEnnemy <= 0f)
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
