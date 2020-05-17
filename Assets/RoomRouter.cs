using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Michsky.UI.ModernUIPack
{
    public class RoomRouter : MonoBehaviour
    {

        public static string sceneName;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void GetRooms()
        {
            //Check Realtime for active rooms*
            //Instantiate a roomCard Prefab foreach available room
        }

        public void RouteToRoom(string roomName)
        {
            AvatarManager.roomName = roomName;
            Loading.sceneString = sceneName;
            SceneManager.LoadScene("Loading");
        }
    }
}
