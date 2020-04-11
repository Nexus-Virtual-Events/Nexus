using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeAdminScript : MonoBehaviour
{

    public string events;
    public string prevEvents;


    public delegate void OnEventChange();
    public static event OnEventChange onEventChange;
    public void RaiseOnEventChange()
    {
        onEventChange();
    }


    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if(events != prevEvents)
        {

        }
    }
}
