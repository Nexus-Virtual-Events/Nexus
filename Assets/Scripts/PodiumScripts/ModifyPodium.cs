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
    }

    private void Awake()
    {
        _realtimeView = GetComponent<RealtimeView>();
        _realtimeTransform = GetComponent<RealtimeTransform>();
    }

    public void SendNewValue(int newPodiumCommand)
    {
        _podiumSync.SetPodium(-1);
        _podiumSync.SetPodium(newPodiumCommand);
    }

    private int prevPodium;

    public void ReceivedNewPodium(int newPodiumReceived)
    {
        Debug.Log("New podium received from ModifyPodium: " + newPodiumReceived.ToString());

        // foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        // {
        //     GameObject.Find("Realtime").GetComponent<AdminPanel>().TurnOffVoice();
        //     if (player.GetComponent<ThirdPersonUserControl>().getID() == prevPodium)
        //         player.GetComponent<ThirdPersonUserControl>().ChangeGlobalVoice(false);
        // }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<ThirdPersonUserControl>().getID() == newPodiumReceived)
            {
                if(player.GetComponent<ThirdPersonUserControl>().GetHasGlobalVoice() == true){
                    player.GetComponent<ThirdPersonUserControl>().ChangeGlobalVoice(false);
                    GameObject.Find("ActionRouter").GetComponent<ActionRouter>().ToggleGlobal(false);
                }
                else{
                    player.GetComponent<ThirdPersonUserControl>().ChangeGlobalVoice(true);
                    GameObject.Find("ActionRouter").GetComponent<ActionRouter>().ToggleGlobal(true);
                    Debug.Log("setting " + player.GetComponent<ThirdPersonUserControl>().getID().ToString() + " to global");

                }
            }
        }

        prevPodium = newPodiumReceived;

        //ActionRouter.GetLocalAvatar().GetComponent<ThirdPersonUserControl>().getID();
    }
}
