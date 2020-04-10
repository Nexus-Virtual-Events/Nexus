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

      // Start is called before the first frame update
      void Start()
      {

      }

      // Update is called once per frame
      void Update()
      {

      }

      public void StartGame()
      {
        PlayerName.playerName = playerName.text;
        Loading.sceneString = scene;
        SceneManager.LoadScene("Loading");
      }
  }
}
