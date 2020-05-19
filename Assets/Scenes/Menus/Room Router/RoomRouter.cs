using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Michsky.UI.ModernUIPack
{
    public class RoomRouter : MonoBehaviour
    {
        public static string sceneString;

        public void RouteToRoom(string roomName)
        {
            if(roomName == "Admin"){
                PlayerPrefs.SetString("adminRoom", "true");
            }
            else{
                PlayerPrefs.SetString("adminRoom", "false");
            }

            PlayerPrefs.SetString("roomName", roomName);
            AvatarCreator.sceneString = sceneString;
            Loading.sceneString = "Avatar Creator";
            SceneManager.LoadScene("Loading");

        }
    }
}
