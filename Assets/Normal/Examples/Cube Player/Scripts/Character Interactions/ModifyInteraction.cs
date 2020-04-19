using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;

namespace Normal.Realtime.Examples
{
    public class ModifyInteraction : MonoBehaviour
    {
        private string _prevInteraction;

        private InteractionSync _interactionSync;

        private RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        public delegate void RemoteInteractionCommand (string newInteractionCommand);
        public event RemoteInteractionCommand OnInteractionsReceived;

        private void Start()
        {
            // Get a reference to the color sync component
            _interactionSync = GetComponent<InteractionSync>();
        }

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        public void SendNewValue(string newInteractionCommand) {
            Debug.Log("Sending value: " + newInteractionCommand);
            _interactionSync.SetInteraction(newInteractionCommand);
        }


        private string[] stringToArray(string s)
        {
            string[] parameters;

            //"(float forwardamount) (float turnamount) (int crouching) (int onGround)"
            parameters = s.Split(' ');
            return parameters;
        }

        public void ReceivedNewInteraction(string newIntreactionReceived) {
            // Check if the target user is me
            string[] parameters = stringToArray(newIntreactionReceived);
            if (parameters[2] == "0") {
                Debug.Log("Received intreaction but not action needed");
                return;
            }

            if (parameters[1] != _realtimeView.ownerID.ToString())
            {
                Debug.Log("Self is not the target");
                return;
            }

            ActionRouter.GetLocalAvatar().GetComponent<ThirdPersonUserControl>().ReactToInteractionChange(gameObject, newIntreactionReceived);
        }
    }
}