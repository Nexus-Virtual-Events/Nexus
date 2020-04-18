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
      public TMP_InputField playerName;
      public string scene;

      public void StartGame()
      {
        PlayerPrefs.SetString("playerName", playerName.text);
        AvatarCreator.sceneString = scene;
        SceneManager.LoadScene("Avatar Creator");
      }
  }
}
