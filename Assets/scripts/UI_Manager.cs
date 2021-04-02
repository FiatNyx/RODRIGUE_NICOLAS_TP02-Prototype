using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager singleton;
    public Text textTour;
	public Text textVie;
	public List<RawImage> uiAttaques;
	public List<RawImage> listeSelectedUI;
	public Text textTimer;


	private void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("Détection de multiples instances du GameManager.");
            return;
        }
		singleton = this;

		




        // Assignation du singleton
       
    }

   

	private void Start()
	{
		float timerJoueur = GameManager.singleton.getTimerJoueur();
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

	public void changeVieText(int vieMax, int vie)
	{
		textVie.text = "Vie : " + vie.ToString() + "/" + vieMax.ToString();
	}


	public void changeSelectedMove(int index)
	{
		foreach (RawImage image in listeSelectedUI)
		{
			image.enabled = false;
		}

		listeSelectedUI[index].enabled = true;
	}
}
