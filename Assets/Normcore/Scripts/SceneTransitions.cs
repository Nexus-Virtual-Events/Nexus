using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTransitions : MonoBehaviour
{

    public TMP_Dropdown sceneSelector;

    public void RouteToBowl()
    {
      if(SceneManager.GetActiveScene().name == "Pinning Scene") SceneManager.LoadScene("Bowl Scene");
    }

    public void RouteToCircle()
    {
      if(SceneManager.GetActiveScene().name == "Bowl Scene") SceneManager.LoadScene("Pinning Scene");
    }

    public void Route()
    {
      if(sceneSelector.value == 0) RouteToBowl();
      if(sceneSelector.value == 1) RouteToCircle();
    }
}
