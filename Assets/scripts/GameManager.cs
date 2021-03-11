using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public static GameManager singleton;
	bool isPlayerTurn = true;

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
			isPlayerTurn = true;
		}
	}

	public bool getPlayerTurn()
	{
		return isPlayerTurn;
	}
}
