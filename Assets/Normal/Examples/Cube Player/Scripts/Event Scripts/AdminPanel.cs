using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

namespace Normal.Realtime.Examples
{
    public class AdminPanel : MonoBehaviour
    {
        public TMP_Text buttonText;
        public TMP_Text buttonHighlightedText;

        public int eventStatus = 0;
        public int eventIndex = 0;
        ModifyEvents eventModifier;

        // Start is called before the first frame update
        void Start()
        {
            eventModifier = GameObject.Find("EventManager").GetComponent<ModifyEvents>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeEventStatus()
        {
            if (buttonText.text == "CHOOSE")
            {
                buttonText.text = "END";
                buttonHighlightedText.text = "CONFIRM";

                eventStatus = 1;

            }
            else
            {
                buttonText.text = "CHOOSE";
                buttonHighlightedText.text = "START";

                eventStatus = 0;
            }

            eventModifier.ChangeEvent(eventIndex, eventStatus);

        }

        public void ChangeSelectedEvent(int selectedIndex)
        {
            eventIndex = selectedIndex;
        }
    }
}