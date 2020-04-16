using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMove : MonoBehaviour
{
    public Vector3 characterMove;
    private Vector3 _prevCharacterMove;

    private MoveSync _moveSync;

    private void Start()
    {
        // Get a reference to the color sync component
        _moveSync = GetComponent<MoveSync>();
    }

    private void Update()
    {
        // If the color has changed (via the inspector), call SetColor on the color sync component.
        if (characterMove != _prevCharacterMove)
        {
            _moveSync.SetMove(characterMove);
            _prevCharacterMove = characterMove;
        }
    }
}