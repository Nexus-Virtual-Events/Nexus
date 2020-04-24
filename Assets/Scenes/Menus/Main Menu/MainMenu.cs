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
        public TMP_InputField _email;
        public TMP_InputField _password;
        public Firebase.Auth.FirebaseUser user;
        public string scene;

        public void Login()
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential("299949821719-6f8dbg9jc00cscu1m34l0v2qutpsmvbm.apps.googleusercontent.com", "RwJoy_KuMfOv-LiR31_hCFPF");
            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.FirebaseUser user = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    user.DisplayName, user.UserId);
            });
        }

        public void StartGame()
        {
            PlayerPrefs.SetString("playerName", user.DisplayName);
            AvatarCreator.sceneString = scene;
            SceneManager.LoadScene("Avatar Creator");
        }
    }
}
