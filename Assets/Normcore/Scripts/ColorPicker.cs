using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour {
    [SerializeField]
    private Color _color;
    private Color _previousColor;

    private ColorSync _colorSync;

    private void Start() {
        // Get a reference to the color sync component
        Debug.Log("Running ColorPicker Start() / Getting ColorSync Component");
        _colorSync = GetComponent<ColorSync>();
        Debug.Log(_colorSync);
    }

    private void Update() {
        // If the color has changed (via the inspector), call SetColor on the color sync component.
        if (_color != _previousColor) {
            Debug.Log("ColorPicker Color Changed!");
            Debug.Log("Previous Color");
            Debug.Log(_previousColor);
            Debug.Log(_color);
            _colorSync.SetColor(_color);
            _previousColor = _color;
        }
    }
}
