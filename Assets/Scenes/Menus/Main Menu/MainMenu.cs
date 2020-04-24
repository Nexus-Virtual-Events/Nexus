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

        public void Register()
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.CreateUserWithEmailAndPasswordAsync(_email.text, _password.text).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                user = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    user.DisplayName, user.UserId);

                StartGame();
            });
        }

        public void Login()
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SignInWithEmailAndPasswordAsync(_email.text, _password.text).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                user = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    user.DisplayName, user.UserId);

                StartGame();
            });
        }

        public void StartGame()
        {
            Debug.Log("StartGame() Called!");
            PlayerPrefs.SetString("playerName", user.DisplayName);
            AvatarCreator.sceneString = scene;
            SceneManager.LoadScene("Avatar Creator");
        }
    }
}
