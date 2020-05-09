﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Normal.Realtime;
using Michsky.UI.ModernUIPack;
using System;


public class UniversalEventCountdown : MonoBehaviour
{

    private DateTime currentTime;
    public int countdownDuration;
    private DateTime startTime;
    private TimeSpan timeLeft;
    public TMP_Text eventStartingNotificationDescription;
    public string scene = "The Circle";
    private EventManager eventManager;
    private GameObject localPlayer;

    private bool eventReceived = false;

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.OnEventsChange.AddListener(ReceiveEvent);
    }

    void ReceiveEvent()
    {
        //if (eventManager.events == null) return;
        //Debug.Log("Received event from universal countdown");
        //Debug.Log(eventManager.events);
        //Debug.Log(eventManager.isInitialized());
        if (eventManager.GetEvents()[0] == '0')
        {
            scene = "The Circle";
            eventReceived = false;
        }
        if (eventManager.GetEvents()[1] == '1')
        {
            currentTime = DateTime.Now;
            startTime = DateTime.Now.AddSeconds(countdownDuration);
            eventReceived = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = DateTime.Now;
        if (eventManager.GetEvents() == null) return;
        if (eventManager.GetEvents()[1] == '1' && eventReceived)
        {
            timeLeft = startTime - currentTime;

            if (timeLeft <= TimeSpan.Zero)
            {
                //GameObject.Find("Realtime").GetComponent<Realtime>().Disconnect();
                //Loading.sceneString = scene;
                //SceneManager.LoadScene("Loading");
            }

            eventStartingNotificationDescription.text = "The main event has opened.";
        }
    }


}
