using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    public class MainMenu : MonoBehaviour
    {
        public string scene;
        public string displayName;

        public void Join()
        {

        }

        public void StartGame()
        {
            Debug.Log("StartGame() Called!");
            PlayerPrefs.SetString("playerName", displayName);
            AvatarCreator.sceneString = scene;
            SceneManager.LoadScene("Avatar Creator");
        }
    }
}
