using System.Collections;
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
    public class EventCountdown : MonoBehaviour
    {

        private DateTime currentTime;
        public int countdownDuration;
        private DateTime startTime;
        private TimeSpan timeLeft;
        public TMP_Text eventStartingNotificationDescription;
        public string scene = "The Bowl";
        private EventManager eventManager;
        private GameObject localPlayer;



        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Event countdown starting!!!!");
            currentTime = DateTime.Now;
            startTime = DateTime.Now.AddSeconds(countdownDuration);
            eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.OnEventsChange.AddListener(ReceiveEvent);
            Debug.Log("Event manager from EventCountdown");
            Debug.Log(eventManager);
        }

        void ReceiveEvent()
        {
            Debug.Log("events from ReceiveEvent");
            Debug.Log(eventManager.events);
            if (eventManager.events[0] == '0')
            {
                scene = "The Bowl";
            }

            countdownDuration = 10;
            if (eventManager.events[1] == '1')
            {
                Debug.Log("Initializing countdown");
            }
            else
            {
                Debug.Log("Resetting countdown");
                currentTime = DateTime.Now;
                startTime = DateTime.Now.AddSeconds(countdownDuration);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (eventManager.events[1] == '1')
            {
                timeLeft = startTime - currentTime;

                if (timeLeft <= TimeSpan.Zero)
                {
                    //foreach (GameObject player in (GameObject.FindGameObjectsWithTag("Player"))) {
                    //    if (player.GetComponent<CubePlayer>().isLocallyOwned()) {
                    //    //Realtime.Destroy(player);
                    //    }
                    GameObject.Find("Realtime").GetComponent<Realtime>().Disconnect();
                }
                Loading.sceneString = scene;
                SceneManager.LoadScene("Loading");
            }
            eventStartingNotificationDescription.text = "A virtual event is starting! You will automatically join the event in " + string.Format("{0}", timeLeft.Seconds) + " seconds.";
        }


    }
}

