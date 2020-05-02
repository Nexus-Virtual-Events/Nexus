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
                _model.lastActionDidChange -= LastActionDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null)
            {
                // Update the mesh render to match the new model
                UpdateMove();

                // Register for events so we'll know if the color changes later
                _model.moveDidChange += MoveDidChange;
                _model.lastActionDidChange += LastActionDidChange;
            }
        }
    }

    private void MoveDidChange(MoveSyncModel model, string move)
    {
        // Update the mesh renderer
        UpdateMove();
    }

    private void LastActionDidChange(MoveSyncModel model, string lastAction)
    {
        // Update the mesh renderer
        TriggerReceivedAction();
    }

    private void UpdateMove()
    {
        GetComponent<UpdateMove>().UpdateCharacterMove(_model.move);
    }

    private void TriggerReceivedAction() {
        GetComponent<ThirdPersonUserControl>().ReceivedRemoteAction(_model.lastAction);
    }

    public void SetLastAction (string action) {
        _model.lastAction = action;
    }

    public void SetMove(string move)
    {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        _model.move = move;
    }
}
