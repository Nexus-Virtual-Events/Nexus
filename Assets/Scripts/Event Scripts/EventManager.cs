using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private string events;
    private string prevEvents;

    public UnityEvent OnEventsChange;

    private bool isInitializedBool = false;
    // Start is called before the first frame update
    void Start()
    {

        if (OnEventsChange == null)
            OnEventsChange = new UnityEvent();

        Debug.Log("EventManager initialized:" + events);
        isInitializedBool = true;

    }

    public bool isInitialized()
    {
        return isInitializedBool;
    }

    public void SetEvents(string eventString)
    {
        events = eventString;
    }

    public string GetEvents()
    {
        return events;
    }


    // Update is called once per frame
    void Update()
    {
        if (events != prevEvents)
        {
            Debug.Log("changing event");
            OnEventsChange.Invoke();
            prevEvents = events;
        }
    }
}
