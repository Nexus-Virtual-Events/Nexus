using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ColorSync : RealtimeComponent {

    public MeshRenderer   _meshRenderer;
    private ColorSyncModel _model;

    private void Start() {
        // Get a reference to the mesh renderer
        Debug.Log("Start MeshRenderer");
        // _meshRenderer = GetComponent<MeshRenderer>();
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
        UpdateMeshRendererColor();
    }

    private void UpdateMeshRendererColor() {
        // Get the color from the model and set it on the mesh renderer.
        Debug.Log("Update MeshRenderer");
        Debug.Log(_meshRenderer);
        _meshRenderer.material.color = _model.color;
    }

    public void SetColor(Color color) {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        Debug.Log("SetColor MeshRenderer:");
        Debug.Log(_meshRenderer);
        Debug.Log("Model Color before SetColor");
        Debug.Log(_model.color);
        _model.color = color;
        Debug.Log("Model Color after SetColor");
        Debug.Log(_model.color);
    }
}
