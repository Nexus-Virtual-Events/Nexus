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

        public void ReceivedNewInteraction(string newInteractionReceived) {
            // Check if the target user is me
            Debug.Log("New interaction:" + newInteractionReceived);

            string[] parameters = stringToArray(newInteractionReceived);
            if (parameters[2] == "0") {
                Debug.Log("Received intreaction but not action needed");
                return;
            }

            Debug.Log("owner ID: " + _realtimeView.ownerID.ToString());
            Debug.Log("parameters[0] " + parameters[0]);
            Debug.Log("parameters[1] " + parameters[1]);

            if (parameters[1] != _realtimeView.ownerID.ToString())
            {
                Debug.Log("Self is the source of the interaction");
                GetComponent<ThirdPersonUserControl>().InitiateInteraction(ActionRouter.GetCurrentCharacter(), newInteractionReceived);
                return;
            }

            Debug.Log("Interaction from another character");
            GetComponent<ThirdPersonUserControl>().ReactToInteractionChange(gameObject, newInteractionReceived);
        }
    }
}