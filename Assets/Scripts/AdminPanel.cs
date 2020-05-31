using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Normal.Realtime;

public class AdminPanel : MonoBehaviour
{

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

            if (focusVoiceButtonText.text == "TURN ON")
            {
                Debug.Log("turning on");
                podiumModifier.SendNewValue(localAvatar.GetComponent<ThirdPersonUserControl>().getID());
                TurnOnVoice();
            }
            else
            {
                Debug.Log("turning off");
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
