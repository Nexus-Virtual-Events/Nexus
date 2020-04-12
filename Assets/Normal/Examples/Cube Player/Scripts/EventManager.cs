using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public string events;
    private string prevEvents;

    public UnityEvent OnEventsChange;
    // Start is called before the first frame update
    void Start()
    {
        events = "00";
        prevEvents = "00";

        if (OnEventsChange == null)
            OnEventsChange = new UnityEvent();
    }


    // Update is called once per frame
    void Update()
    {
        if (events != prevEvents)
        {
            OnEventsChange.Invoke();
            prevEvents = events;
        }
    }
}
