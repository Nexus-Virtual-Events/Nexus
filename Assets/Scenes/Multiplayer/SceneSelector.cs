using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.UI;
// using TMPro;

public class SceneSelector : MonoBehaviour
{

    // public TMP_Dropdown sceneSelector;

    public void LoadCircle()
    {
      if(SceneManager.GetActiveScene().name == "Bowl Scene") SceneManager.LoadScene("Pinning Scene");
    }

    public void LoadBowl()
    {
      if(SceneManager.GetActiveScene().name == "Pinning Scene") SceneManager.LoadScene("Bowl Scene");
    }

    // public void SelectScene()
    // {
    //   if(SceneManager.GetActiveScene().name != sceneSelector.options[sceneSelector.value].text) SceneManager.LoadScene(sceneSelector.options[sceneSelector.value].text);
    // }
}
