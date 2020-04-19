using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;

namespace Normal.Realtime.Examples
{
    public class ModifyInteraction : MonoBehaviour
    {
        private string interaction;
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
            interaction = "0 0 0";
            _prevInteraction = "0 0 0";


        }

        public void SetCurrentInteraction (string nInteraction) {
            Debug.Log("Updating current interaction: " + nInteraction);
            Debug.Log("IsLocal?" + _realtimeView.isOwnedLocally);
            Debug.Log("Interaction sync: " + _interactionSync);
            string str = UnityEngine.StackTraceUtility.ExtractStackTrace ();
            Debug.Log(str);
            interaction = nInteraction;
        }

        public string GetCurrentInteraction () {
            return interaction;
        }

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        public void SendNewValue(string newInteractionCommand) {
            _interactionSync.SetInteraction(interaction);
        }

        public void ReceivedNewInteraction(string newIntreactionReceived) {
            if (!_realtimeView.isOwnedLocally) {
                Debug.Log("received intreaction but not local");
                return;
            }

            OnInteractionsReceived.Invoke(newIntreactionReceived);
        }

        private void Update()
        {
            // _realtimeTransform.RequestOwnership();

            // if (_interactionSync == null || interaction == null)
            // {
            //     _interactionSync = GameObject.FindObjectOfType<InteractionSync>();
            //     Debug.Log("Interaction sync found: " + _interactionSync);
            // }
            
            // if (_interactionSync)
            // {
                
            //     if (interaction != _prevInteraction)
            //     {
            //         Debug.Log("[Modify Interaction] " + _prevInteraction + " > " + interaction);
            //         _interactionSync.SetInteraction(interaction);
            //         _prevInteraction = interaction;
                    
            //         interaction = "0 0 0";
            //     }
                
            // }
            // If the color has changed (via the inspector), call SetColor on the color sync component.

        }
    }
}