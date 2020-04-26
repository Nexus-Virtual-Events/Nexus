using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Normal.Realtime;

public class AdminPanel : MonoBehaviour
{
    public TMP_Text eventSelectButtonText;
    public TMP_Text eventSelectButtonHighlightedText;

    public int eventStatus = 0;
    public int eventIndex = 0;
    ModifyEvents eventModifier;

    public TMP_Text focusCameraButtonText;
    public TMP_Text focusVoiceButtonText;

    private GameObject localAvatar;

    // Start is called before the first frame update
    void Start()
    {
        eventModifier = GameObject.Find("EventManager").GetComponent<ModifyEvents>();
        localAvatar = ActionRouter.GetLocalAvatar();
    }

    // Update is called once per frame
    void Update()
    {
        if(localAvatar == null)
        {
           localAvatar = ActionRouter.GetLocalAvatar();
        }
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

            if (localAvatar) {
                ModifyPodium podiumModifier = localAvatar.GetComponent<ModifyPodium>();
                podiumModifier.SendNewValue(ActionRouter.GetLocalAvatar().GetComponent<ThirdPersonUserControl>().getID());
            }
        }
        else
        {
            focusCameraButtonText.text = "FOCUS";

            eventModifier.ChangeCamera(0);

        }
    }

    public void ToggleFocusVoiceMode()
    {   
        ModifyPodium podiumModifier;

        if (localAvatar) {
            podiumModifier = localAvatar.GetComponent<ModifyPodium>();
            podiumModifier.SendNewValue(ActionRouter.GetLocalAvatar().GetComponent<ThirdPersonUserControl>().getID());
        }
        else {
            Debug.Log("No local avatar found");
            return;
        }
    
        if (focusVoiceButtonText.text == "LOCAL")
        {
            focusVoiceButtonText.text = "GLOBAL";
            
        }
        else
        {
            focusCameraButtonText.text = "LOCAL";

            podiumModifier.SendNewValue(-1);
        }
    }
}
