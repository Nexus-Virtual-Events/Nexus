using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
using Normal.Realtime;

public class ModifyPodium : MonoBehaviour
{

    private PodiumSync _podiumSync;

    private RealtimeView _realtimeView;
    private RealtimeTransform _realtimeTransform;

    public delegate void RemoteInteractionCommand(string newInteractionCommand);
    public event RemoteInteractionCommand OnInteractionsReceived;

    private void Start()
    {
        // Get a reference to the color sync component
        _podiumSync = GetComponent<PodiumSync>();
        Debug.Log("Podium sync:" + _podiumSync.ToString());

    }

    private void Awake()
    {
        _realtimeView = GetComponent<RealtimeView>();
        _realtimeTransform = GetComponent<RealtimeTransform>();
    }

    public void SendNewValue(int newPodiumCommand)
    {
        _podiumSync.SetPodium(newPodiumCommand);
    }

    // public void ResetPodium(){
    //     Debug.Log("reset podium called");
    //     _podiumSync.SetPodium(-1);
    // }

    private int prevPodium;

    public void ReceivedNewPodium(int newPodiumReceived)
    {
        Debug.Log("New podium received from ModifyPodium: " + newPodiumReceived.ToString());

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            GameObject.Find("Realtime").GetComponent<AdminPanel>().TurnOffVoice();
            if (player.GetComponent<ThirdPersonUserControl>().getID() == prevPodium)
                player.GetComponent<ThirdPersonUserControl>().ChangeGlobalVoice(false);
            GameObject.Find("ActionRouter").GetComponent<ActionRouter>().ToggleGlobal(true);
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<ThirdPersonUserControl>().getID() == newPodiumReceived)
            {
                player.GetComponent<ThirdPersonUserControl>().ChangeGlobalVoice(true);
                if(player.GetComponent<ThirdPersonUserControl>().isLocallyOwned()){
                    GameObject.Find("ActionRouter").GetComponent<ActionRouter>().ToggleGlobal(false);
                }
            }
        }

        prevPodium = newPodiumReceived;

        //ActionRouter.GetLocalAvatar().GetComponent<ThirdPersonUserControl>().getID();
    }
}
