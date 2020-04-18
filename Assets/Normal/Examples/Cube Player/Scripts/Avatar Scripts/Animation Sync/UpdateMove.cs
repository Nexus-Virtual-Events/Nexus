using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Normal.Realtime.Examples
{
    public class UpdateMove : MonoBehaviour
    {
        public string characterMove;
        private string _prevCharacterMove;

        private MoveSync _moveSync;

        private RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        private void Start()
        {
            // Get a reference to the color sync component
            _moveSync = GetComponent<MoveSync>();
        }

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        private void Update()
        {

            if (!_realtimeView.isOwnedLocally)
                return;

            _realtimeTransform.RequestOwnership();

            if (_moveSync == null || characterMove == null)
            {
                _moveSync = GameObject.FindObjectOfType<MoveSync>();
                characterMove = "0 0 0 0 0 0";
            }
            else
            {
                

                if (characterMove != _prevCharacterMove)
                {
                    _moveSync.SetMove(characterMove);
                    _prevCharacterMove = characterMove;
                }
            }
            // If the color has changed (via the inspector), call SetColor on the color sync component.
          
        }
    }
}