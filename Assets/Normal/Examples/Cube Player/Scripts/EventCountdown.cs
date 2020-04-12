using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    public class EventCountdown : MonoBehaviour
    {

        private float currentTime = 0f;
        public float countdownDuration = 60f;
        public TMP_Text eventStartingNotificationDescription;
        public string scene = "The Bowl";
        private EventManager eventManager;



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
            if(eventManager.events[0] == '0')
            {
                scene = "The Bowl";
            }

            if(eventManager.events[1] == '1')
            {
                countdownDuration = 60;
            }
            else
            {
                countdownDuration = 0;  
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
                    Loading.sceneString = scene;
                    SceneManager.LoadScene("Loading");
                }
                eventStartingNotificationDescription.text = "A virtual event is starting! You will automatically join the event in " + currentTime.ToString("0") + " seconds.";
            }


        }
    }
}