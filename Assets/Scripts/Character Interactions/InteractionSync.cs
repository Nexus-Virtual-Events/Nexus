using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;


public class InteractionSync : RealtimeComponent
{

    private InteractionSyncModel _model;

    private void Start()
    {
        // Get a reference to the mesh renderer
        //_meshRenderer = GetComponent<MeshRenderer>();
        //_characterMove = GetComponent<UpdateMove>.
    }

    private InteractionSyncModel model
    {
        set
        {
            if (_model != null)
            {
                // Unregister from events
                _model.interactionDidChange -= InteractionDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null)
            {
                // Update the mesh render to match the new model
                UpdateInteraction();

                // Register for events so we'll know if the color changes later
                _model.interactionDidChange += InteractionDidChange;
            }
        }
    }

    private void InteractionDidChange(InteractionSyncModel model, string value)
    {
            
        // Update the mesh renderer
        // UpdateInteraction();
            Debug.Log("Received intearction: " + value);
        if (value == "") { return; }
        Debug.Log("Received intearction: " + value);
        GetComponent<ModifyInteraction>().ReceivedNewInteraction(value);
    }

    private void UpdateInteraction()
    {
        // Get the color from the model and set it on the mesh renderer.
        if (_model.interaction == "") {
            Debug.LogWarning("Empty intreaction value");
            return;
        }
            
            
    }

    public void SetInteraction(string interaction)
    {
        Debug.Log("Setting to:" + interaction);
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        _model.interaction = interaction;
    }
}
