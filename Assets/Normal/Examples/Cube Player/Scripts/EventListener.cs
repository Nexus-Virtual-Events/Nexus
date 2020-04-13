using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.OnEventsChange.AddListener(Ping);
    }

    void Ping()
    {
        UnityEngine.Debug.Log("EventManager Ping Notification");
        UnityEngine.Debug.Log(eventManager.events);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
