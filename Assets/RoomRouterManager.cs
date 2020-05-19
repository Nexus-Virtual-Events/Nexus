using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRouterManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject adminRoom;
    void Start()
    {
        if(PlayerPrefs.GetString("isAdmin") != "true"){
            adminRoom.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void JoinRegularRoom(){
    //     PlayerPrefs.SetString("adminRoom", "true");
    // }
}
