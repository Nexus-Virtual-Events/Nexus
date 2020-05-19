using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRoomRouter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] userRooms;
    void Start()
    {
        if(PlayerPrefs.GetString("adminRoom") == "true"){
            foreach(GameObject room in userRooms){
                room.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
