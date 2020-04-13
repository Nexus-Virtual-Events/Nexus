﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Normal.Realtime;
using Michsky.UI.ModernUIPack;
using System;

namespace Normal.Realtime.Examples
{
    public class UniversalEventCountdown : MonoBehaviour
    {

        private DateTime currentTime;
        public int countdownDuration;
        private DateTime startTime;
        private TimeSpan timeLeft;
        public TMP_Text eventStartingNotificationDescription;
        public string scene = "The Bowl";
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
            if (eventManager.events == null) return;
            if (eventManager.events[0] == '0')
            {
                scene = "The Bowl";
                eventReceived = false;
            }
            if (eventManager.events[1] == '1')
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
            if (eventManager.events == null) return;
            if (eventManager.events[1] == '1' && eventReceived)
            {
                timeLeft = startTime - currentTime;

                if (timeLeft <= TimeSpan.Zero)
                {
                    GameObject.Find("Realtime").GetComponent<Realtime>().Disconnect();
                    Loading.sceneString = scene;
                    SceneManager.LoadScene("Loading");
                }

                eventStartingNotificationDescription.text = "A virtual event is starting! You will automatically join the event in " + string.Format("{0}", timeLeft.Seconds) + " seconds.";
            }
        }


    }
}