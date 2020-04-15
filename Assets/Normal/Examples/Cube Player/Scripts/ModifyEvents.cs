using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Normal.Realtime;

namespace Normal.Realtime.Examples
{
    public class ModifyEvents : MonoBehaviour
    {

        private string _events;
        private string _prevEvents;

        private EventSync _eventSync;

        private RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        private void Start()
        {
            _eventSync = GameObject.FindObjectOfType<EventSync>();
            _events = "00";
            _prevEvents = "00";
            Debug.Log("Start() events from ModifyEvents:" + _events);
        }

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        public void ChangeEvent(int eventIndex, int eventStatus)
        {
            _events = eventIndex.ToString() + eventStatus.ToString();
        }


        public void Update()
        {
            //if (!_realtimeView.isOwnedLocally)
            //    return;

            //_realtimeTransform.RequestOwnership();

            if (_eventSync == null)
            {
                _eventSync = GameObject.FindObjectOfType<EventSync>();
            }
            else
            {
                if (_prevEvents != _events)
                {
                    Debug.Log("ModifEvents changing the event to:" + _events);
                    _eventSync.SetEvent(_events);
                    _prevEvents = _events;
                }
            }
        }
    }
}

