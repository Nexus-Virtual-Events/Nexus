using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.UI;
// using TMPro;

namespace Michsky.UI.ModernUIPack
{
  public class SceneSelector : MonoBehaviour
  {

      // public TMP_Dropdown sceneSelector;
      public Animator transition;
      public float transitionDuration = 1f;

      public void LoadCircle()
      {
        StartCoroutine(TransitionToScene("Pinning Scene"));
      }

      public void LoadBowl()
      {
        StartCoroutine(TransitionToScene("Bowl Scene"));
      }

      IEnumerator TransitionToScene(string scene)
      {
        // transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionDuration);

        if(SceneManager.GetActiveScene().name != scene)
        {
          Debug.Log("Transitioning to " + scene);
          Loading.sceneString = scene;
          SceneManager.LoadScene("Loading");
        }

      }

      // public void SelectScene()
      // {
      //   if(SceneManager.GetActiveScene().name != sceneSelector.options[sceneSelector.value].text) SceneManager.LoadScene(sceneSelector.options[sceneSelector.value].text);
      // }
  }
}
