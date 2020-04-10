using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class InfoSync : Realtime
{
    // Start is called before the first frame update
    private TextMesh _nameText;
    private InfoSyncModel _model;

    void Start()
    {
        GameObject _nameObject = GameObject.Find("Name Text");
        _nameText = _nameObject.GetComponent<TextMesh>();
    }

    private InfoSyncModel model {
        set {
            if (_model != null) {
                // Unregister from events
                _model.nameDidChange -= NameDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null) {
                // Update the mesh render to match the new model
                UpdateName();

                // Register for events so we'll know if the color changes later
                _model.nameDidChange += NameDidChange;
            }
        }
    }

     private void NameDidChange(InfoSyncModel model, string value) {
        // Update the mesh renderer
        UpdateName();
    }


    private void UpdateName() {
        // Get the color from the model and set it on the mesh renderer.
        _nameText.text = _model.name;
    }

    public void SetName(string n) {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        _model.name = n;
    }
}
