﻿using System.Collections;
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

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();   
    }

    void UpdateTimer()
    {
        float time = GameManager.singleton.getTimerJoueur();
        time = Mathf.Round(time * 10f) / 10f;
        textTimer.text = time.ToString();

    }

    public void changeTurnText()
    {
        //Changer le texte du tour
    }
}
