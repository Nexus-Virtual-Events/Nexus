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

        // Start is called before the first frame update
        void Start()
        {
            currentTime = countdownDuration;
        }

        // Update is called once per frame
        void Update()
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