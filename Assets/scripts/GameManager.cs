using System.Collections;
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

	List<Transform> listeEnnemis = new List<Transform>();
	public GameObject conteneurEnnemi;
	int indexEnnemy = 0;

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
		foreach (Transform child in conteneurEnnemi.transform)
			listeEnnemis.Add(child);
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
		
		if(isPlayerTurn == false)
		{
			if (indexEnnemy > 0)
			{
				if(listeEnnemis[indexEnnemy - 1] != null)
				{
					listeEnnemis[indexEnnemy - 1].GetComponent<ennemyBasic>().isThisEnnemyTurn = false;
				}
				
			}

			if (indexEnnemy >= listeEnnemis.Count)
			{
				timerJoueur = tempsTourJoueur;
				isPlayerTurn = true;
				UI_Manager.singleton.changeTurnText(true);
				isTimerStopped = false;
				indexEnnemy = 0;
			}
			else
			{
				timerEnnemy = tempsTourEnnemy;
				

				listeEnnemis[indexEnnemy].GetComponent<ennemyBasic>().isThisEnnemyTurn = true;
				indexEnnemy += 1;
			}
			
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

	public void killEnnemy(Transform ennemy)
	{
		if (isPlayerTurn)
		{
			listeEnnemis.Remove(ennemy);
		}
		else
		{
			StartCoroutine(WaitForPlayerTurn(ennemy));
		}
		
	}

	IEnumerator WaitForPlayerTurn(Transform ennemy)
	{
		while(isPlayerTurn == false)
		{
			yield return null;
		}

		listeEnnemis.Remove(ennemy);

	}
}
