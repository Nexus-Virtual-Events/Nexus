using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Michsky.UI.ModernUIPack
{
  public class Loading : MonoBehaviour
  {
      public static string sceneString;

      // Start is called before the first frame update
      void Start()
      {
          StartCoroutine(LoadAsyncOperation());
      }

      IEnumerator LoadAsyncOperation()
      {
        AsyncOperation loadedScene = SceneManager.LoadSceneAsync(sceneString);

        while(loadedScene.progress < 1)
        {
          yield return new WaitForEndOfFrame();
        }
      }

      // Update is called once per frame
      void Update()
      {

      }
  }
}
