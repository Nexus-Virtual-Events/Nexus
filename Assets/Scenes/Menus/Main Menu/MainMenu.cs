using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using System.Text;


namespace Michsky.UI.ModernUIPack
{
    public class MainMenu : MonoBehaviour
    {
        public TMP_InputField _email;
        public TMP_InputField _password;
        public Text _warningText;
        //public Firebase.Auth.FirebaseUser user;
        public string scene;

        private void Start()
        {
            _warningText.text = "";
        }



        //public void Register()
        //{
        //    Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        //    auth.CreateUserWithEmailAndPasswordAsync(_email.text, _password.text).ContinueWith(task =>
        //    {
        //        if (task.IsCanceled)
        //        {
        //            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
        //            return;
        //        }
        //        if (task.IsFaulted)
        //        {
        //            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
        //            return;
        //        }

        //        // Firebase user has been created.
        //        user = task.Result;
        //        Debug.LogFormat("Firebase user created successfully: {0} ({1})",
        //            user.DisplayName, user.UserId);

        //        StartGame();
        //    });
        //}
        IEnumerator Post(string url, string bodyJsonString)
        {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            Debug.Log("Status Code: " + request.responseCode);
            yield return request.responseCode;
        }

        public void Login()
        {
            //StartGame();
            Debug.Log("login activated with " + _email.text + _password.text);
            string jsonString = "{\"email\":" + _email.text + "\"password\":" + _password.text + "}";
            //CoroutineWithData cd = new CoroutineWithData(this, Post("http://the-nexus.herokuapp.com/authenticate_with_unity", jsonString));
            //yield return cd.coroutine;
            StartCoroutine(Post("http://the-nexus.herokuapp.com/authenticate_with_unity", jsonString));
            //Debug.Log("result is " + cd.result);  //  'success' or 'fail'
            
            //Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            //auth.SignInWithEmailAndPasswordAsync(_email.text, _password.text).ContinueWith(task =>
            //{
            //    if (task.IsCanceled)
            //    {
            //        Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
            //        return;
            //    }
            //    if (task.IsFaulted)
            //    {
            //        Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            //        return;
            //    }

            //    user = task.Result;
            //    Debug.LogFormat("User signed in successfully: {0} ({1})",
            //        user.DisplayName, user.UserId);

            //    StartGame();
            //});
        }

        public void StartGame()
        {
            Debug.Log("StartGame() Called!");
            PlayerPrefs.SetString("playerName", _email.text);
            AvatarCreator.sceneString = scene;
            SceneManager.LoadScene("Avatar Creator");
        }
    }

    public class CoroutineWithData
    {
        public Coroutine coroutine { get; private set; }
        public object result;
        private IEnumerator target;
        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                result = target.Current;
                yield return result;
            }
        }
    }
}
