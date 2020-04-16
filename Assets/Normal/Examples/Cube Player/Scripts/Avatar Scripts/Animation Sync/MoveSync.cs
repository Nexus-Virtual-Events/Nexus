using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class MoveSync : RealtimeComponent
{

    private MoveSyncModel _model;

    private void Start()
    {
        // Get a reference to the mesh renderer
        //_meshRenderer = GetComponent<MeshRenderer>();
        //_characterMove = GetComponent<UpdateMove>.
    }

    private MoveSyncModel model
    {
        set
        {
            if (_model != null)
            {
                // Unregister from events
                _model.moveDidChange -= MoveDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null)
            {
                // Update the mesh render to match the new model
                UpdateMove();

                // Register for events so we'll know if the color changes later
                _model.moveDidChange += MoveDidChange;
            }
        }
    }

    private void MoveDidChange(MoveSyncModel model, Vector3 value)
    {
        // Update the mesh renderer
        UpdateMove();
    }

    private void UpdateMove()
    {
        // Get the color from the model and set it on the mesh renderer.
        GetComponent<UpdateMove>().characterMove = _model.move;
    }

    public void SetMove(Vector3 move)
    {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        _model.move = move;
    }
}