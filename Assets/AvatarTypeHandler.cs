using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarTypeHandler : MonoBehaviour
{
    public MonoBehaviour[] componentsToEnable;
    public GameObject[] objectsToActivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAsSelf () {

        foreach (MonoBehaviour c in componentsToEnable){
            c.enabled = true;
        }

        foreach (GameObject o in objectsToActivate){
            o.SetActive(true);
        }
    }
}
