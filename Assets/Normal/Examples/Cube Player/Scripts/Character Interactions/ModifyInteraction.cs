using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;

namespace Normal.Realtime.Examples
{
    public class ModifyInteraction : MonoBehaviour
    {
        public string interaction;
        private string _prevInteraction;

        private InteractionSync _interactionSync;

        private RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        public UnityEvent OnInteractionsChange;

        private void Start()
        {
            // Get a reference to the color sync component
            _interactionSync = GetComponent<InteractionSync>();
            interaction = "0 0 0";
            _prevInteraction = "0 0 0";

            if (OnInteractionsChange == null)
                OnInteractionsChange = new UnityEvent();

        }

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        private void Update()
        {

            if (!_realtimeView.isOwnedLocally)
                return;

            _realtimeTransform.RequestOwnership();

            if (_interactionSync == null || interaction == null)
            {
                _interactionSync = GameObject.FindObjectOfType<InteractionSync>();
                interaction = "0 0 0";
            }
            else
            {
                
                if (interaction != _prevInteraction)
                {
                    _interactionSync.SetInteraction(interaction);
                    _prevInteraction = interaction;
                    OnInteractionsChange.Invoke();
                    interaction = "0 0 0";

                }
                
            }
            // If the color has changed (via the inspector), call SetColor on the color sync component.

        }
    }
}