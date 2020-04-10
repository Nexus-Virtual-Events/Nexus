using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ColorSync : RealtimeComponent {

    private MeshRenderer   _meshRenderer;
    private ColorSyncModel _model;

    private void Start() {
        // Get a reference to the mesh renderer
        Debug.Log("Running ColorSync Start()...");
        Debug.Log("Getting MeshRenderer...");
        _meshRenderer = GetComponent<MeshRenderer>();
        Debug.Log(_meshRenderer);
    }

    private ColorSyncModel model {
        set {
            if (_model != null) {
                // Unregister from events
                _model.colorDidChange -= ColorDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null) {
                // Update the mesh render to match the new model
                UpdateMeshRendererColor();

                // Register for events so we'll know if the color changes later
                _model.colorDidChange += ColorDidChange;
            }
        }
    }

    private void ColorDidChange(ColorSyncModel model, Color value) {
        // Update the mesh renderer
        Debug.Log("Color Changed! Updating MeshRenderer Color...");
        UpdateMeshRendererColor();
    }

    private void UpdateMeshRendererColor() {
        Debug.Log("Updating MeshRenderer Color!!!");
        if(_meshRenderer == null)
        {
          Debug.Log("MeshRenderer is NULL!!!");
        }
        else
        {
          // Get the color from the model and set it on the mesh renderer.
          Debug.Log("MeshRenderer IS SET!!!");
          Debug.Log("New MeshRenderer Color...");
          Debug.Log(_model.color);
          Debug.Log("Previous MeshRenderer Color...");
          Debug.Log(_meshRenderer.material.color);
          _meshRenderer.material.color = _model.color;
          Debug.Log("New MeshRenderer Color...");
          Debug.Log(_meshRenderer.material.color);
        }
    }

    public void SetColor(Color color) {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        Debug.Log("Running ColorSync SetColor()...");
        Debug.Log("New Color");
        Debug.Log(color);
        Debug.Log("Previous Model Color");
        Debug.Log(_model.color);
        _model.color = color;
        Debug.Log("New Model Color");
        Debug.Log(_model.color);
    }
}
