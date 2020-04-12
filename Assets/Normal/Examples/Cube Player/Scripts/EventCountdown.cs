using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Normal.Realtime;
using Michsky.UI.ModernUIPack;

namespace Normal.Realtime.Examples {
        public class EventCountdown : MonoBehaviour
        {

            private float currentTime = 0f;
            public float countdownDuration;
            public TMP_Text eventStartingNotificationDescription;
            public string scene = "The Bowl";
            private EventManager eventManager;
            private GameObject localPlayer;



            // Start is called before the first frame update
            void Start()
            {
                Debug.Log("Event countdown starting!!!!");
                currentTime = countdownDuration;
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

                countdownDuration = 10.0f;
                if (eventManager.events[1] == '1')
                {
                    Debug.Log("Initializing countdown");
                }
                else
                {
                    Debug.Log("Resetting countdown");
                    currentTime = countdownDuration;
                }

            }


            // Update is called once per frame
            void Update()
            {
                if (eventManager.events[1] == '1')
                {
                    if (currentTime > 0)
                    {
                        currentTime -= 1 * Time.deltaTime;
                    }
                    else
                    {
                        foreach (GameObject player in (GameObject.FindGameObjectsWithTag("Player"))) {
                            if (player.GetComponent<CubePlayer>().isLocallyOwned()) {
                            Realtime.Destroy(player);
                            }
                    }
                        Loading.sceneString = scene;
                        SceneManager.LoadScene("Loading");
                    }
                    eventStartingNotificationDescription.text = "A virtual event is starting! You will automatically join the event in " + currentTime.ToString("0") + " seconds.";
                }


            }
        }
    }
