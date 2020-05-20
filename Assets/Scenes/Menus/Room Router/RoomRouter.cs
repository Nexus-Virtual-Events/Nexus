using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Michsky.UI.ModernUIPack
{
    public class RoomRouter : MonoBehaviour
    {
        public static string sceneString;
        public int roomIndex;
        public void RouteToRoom()
        {
            if(roomIndex == 5){
                PlayerPrefs.SetString("adminRoom", "true");
            }
            else{
                PlayerPrefs.SetString("adminRoom", "false");
            }

            if(roomIndex != 5){
                PlayerPrefs.SetString("roomName", "Room "+(roomIndex+1).ToString());
            }
            else{
                PlayerPrefs.SetString("roomName", "Admin "+(roomIndex+1).ToString());
            }
            AvatarCreator.sceneString = sceneString;
            Loading.sceneString = "Avatar Creator";
            SceneManager.LoadScene("Loading");

        }

        public void RouteWithIndex(int index)
        {
            if (index == 5)
            {
                PlayerPrefs.SetString("adminRoom", "true");
            }
            else
            {
                PlayerPrefs.SetString("adminRoom", "false");
            }

            if (index != 5)
            {
                PlayerPrefs.SetString("roomName", "Room " + (index + 1).ToString());
            }
            else
            {
                PlayerPrefs.SetString("roomName", "Admin " + (index + 1).ToString());
            }
            AvatarCreator.sceneString = sceneString;
            Loading.sceneString = "Avatar Creator";
            SceneManager.LoadScene("Loading");
        }
    }
}
