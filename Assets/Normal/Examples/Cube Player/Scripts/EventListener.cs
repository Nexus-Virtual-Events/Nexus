using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = GameObject.Find("EventManagerObject").GetComponent<EventManager>();
        eventManager.OnEventsChange.AddListener(Ping);
    }

    void Ping()
    {
        Debug.Log(eventManager.events);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
