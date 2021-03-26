using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager singleton;
    public Text textTour;

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

    public Text textTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateTimer(float timeRemaining)
    {

        //float time = GameManager.singleton.getTimerJoueur();
        timeRemaining = Mathf.Round(timeRemaining * 10f) / 10f;
        textTimer.text = timeRemaining.ToString();
        
       

    }

    public void changeTurnText(bool isTurnJoueur)
    {
		if(isTurnJoueur)
		{
			textTour.text = "Tour du joueur";
		}
		else
		{
			textTour.text = "Tour des ennemis";
		}
    }
}
