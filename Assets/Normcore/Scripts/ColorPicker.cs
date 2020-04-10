using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour {
    [SerializeField]
    private Color _color;
    private Color _previousColor;

    public ColorSync _colorSync;

    private void Start() {
        // Get a reference to the color sync component
        // _colorSync = GetComponent<ColorSync>();
    }

    private void Update() {
        // If the color has changed (via the inspector), call SetColor on the color sync component.
        if (_color != _previousColor) {
            Debug.Log("Model Color in SetColor");
            Debug.Log(_color);
            _colorSync.SetColor(_color);
            _previousColor = _color;
        }
    }
}
