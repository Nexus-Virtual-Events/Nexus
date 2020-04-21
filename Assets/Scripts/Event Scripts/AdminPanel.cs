using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using Normal.Realtime;

public class AdminPanel : MonoBehaviour
{
    public TMP_Text eventSelectButtonText;
    public TMP_Text eventSelectButtonHighlightedText;

    public int eventStatus = 0;
    public int eventIndex = 0;
    ModifyEvents eventModifier;

    public TMP_Text focusCameraButtonText;

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
        if (eventSelectButtonText.text == "CHOOSE")
        {
            eventSelectButtonText.text = "END";
            eventSelectButtonHighlightedText.text = "CONFIRM";

            eventStatus = 1;

        }
        else
        {
            eventSelectButtonText.text = "CHOOSE";
            eventSelectButtonHighlightedText.text = "START";

            eventStatus = 0;
        }

        eventModifier.ChangeEvent(eventIndex, eventStatus);

    }

    public void ChangeSelectedEvent(int selectedIndex)
    {
        eventIndex = selectedIndex;
    }

    public void ToggleFocusCameraMode()
    {
        if (focusCameraButtonText.text == "FOCUS")
        {
            focusCameraButtonText.text = "UNFOCUS";

            eventModifier.ChangeCamera(1);
        }
        else
        {
            focusCameraButtonText.text = "FOCUS";

            eventModifier.ChangeCamera(0);
        }
    }
}
