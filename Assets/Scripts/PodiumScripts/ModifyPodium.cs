using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
using Normal.Realtime;

public class ModifyPodium : MonoBehaviour
{
    private string _prevPodium;

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
        Debug.Log("Sending value: " + newPodiumCommand);
        _podiumSync.SetPodium(newPodiumCommand);
    }

    public void ReceivedNewPodium(int newPodiumReceived)
    {
        ActionRouter.GetLocalAvatar().GetComponent<ThirdPersonUserControl>().ReactToPodiumChange(newPodiumReceived);
    }
}
