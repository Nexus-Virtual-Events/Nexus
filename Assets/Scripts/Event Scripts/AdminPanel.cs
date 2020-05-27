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
    ModifyPodium podiumModifier;

    public GameObject playersTab;
    public GameObject playerManagementCardPrefab;
    public GameObject[] players;

    // Start is called before the first frame update

    public GameObject podium;
    void Start()
    {
        eventModifier = GameObject.Find("EventManager").GetComponent<ModifyEvents>();
        localAvatar = ActionRouter.GetLocalAvatar();
        podiumModifier = podium.GetComponent<ModifyPodium>();
        Debug.Log("Podium modifier:" + podiumModifier.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (localAvatar == null)
        {
            localAvatar = ActionRouter.GetLocalAvatar();
        }
    }

    ModifyPodium GetPodiumModifier () {
        return localAvatar.GetComponent<ModifyPodium>();
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

    public void FocusCamera()
    {
        focusCameraButtonText.text = "UNFOCUS";
    }

    public void UnfocusCamera()
    {
        focusCameraButtonText.text = "FOCUS";
    }

    //private void ChangeVoiceButton()
    //{
    //    if (focusVoiceButtonText.text == "TURN ON")
    //    {
    //        focusVoiceButtonText.text = "TURN OFF";
    //    }
    //    else
    //    {
    //    }
    //}

    public void TurnOnVoice() {
        focusVoiceButtonText.text = "TURN OFF";
    }

    public void TurnOffVoice()
    {
        focusVoiceButtonText.text = "TURN ON";
    }


    public void ToggleFocusVoiceMode()
    {
        Debug.Log("toggle focus voice mode called");
        if (localAvatar)
        {
            Debug.Log("requirements set");

            if (localAvatar.GetComponent<ThirdPersonUserControl>().GetHasGlobalVoice() == false)
            {
                Debug.Log("turning on");
                // podiumModifier.ResetPodium();
                podiumModifier.SendNewValue(localAvatar.GetComponent<ThirdPersonUserControl>().getID());
                TurnOnVoice();
            }
            else
            {
                Debug.Log("turning off");
                // podiumModifier.ResetPodium();
                podiumModifier.SendNewValue(-1);
                TurnOffVoice();
            }
        }
        // else
        // {
            // Debug.Log("No local avatar or podium modifier found");
            // podiumModifier = GameObject.Find("Podium").GetComponent<ModifyPodium>();
        // }
    }

    public void GetPlayers()
    {
        foreach (Transform child in playersTab.transform)
        {
            Destroy(child.gameObject);
        }

        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            GameObject playerManagementCard = Instantiate(playerManagementCardPrefab);

            string playerName = player.transform.Find("Player Name").GetComponent<TMP_Text>().text;
            playerManagementCard.transform.Find("PlayerManagementCard").GetComponent<TMP_Text>().text = playerName;

            playerManagementCard.transform.SetParent(playersTab.transform, false);
        }
    }
}
