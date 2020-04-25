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
        public TMP_Text _warningText;

        public string scene;
        public string displayName;

        private void Start()
        {
            _warningText.text = "";
        }

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
        }

        public void StartGame()
        {
            Debug.Log("StartGame() Called!");
            PlayerPrefs.SetString("playerName", displayName);
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
