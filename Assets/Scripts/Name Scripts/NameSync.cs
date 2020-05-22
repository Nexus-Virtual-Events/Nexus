using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;

public class NameSync : RealtimeComponent
{

    public TMP_Text _playerNameText;
    private NameSyncModel _model;

    private NameSyncModel model
    {
        set
        {
            if (_model != null)
            {
                // Unregister from events
                _model.nameDidChange -= NameDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null)
            {
                // Update the mesh render to match the new model
                UpdateName();

                // Register for events so we'll know if the color changes later
                _model.nameDidChange += NameDidChange;
            }
        }
    }

    private void NameDidChange(NameSyncModel model, string value)
    {
        // Update the mesh renderer
        UpdateName();
    }

    private void UpdateName()
    {
        // Get the color from the model and set it on the mesh renderer.
        _playerNameText.text = _model.name;
    }

    public void SetName(string name)
    {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        _model.name = name;
    }

    public string GetName()
    {
        return _model.name;
    }
}
