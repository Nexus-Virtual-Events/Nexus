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

    public class LoginInfo
    {
        public int code;
        public string admin;
        public string name;

    }

    public class MainMenu : MonoBehaviour
    {
        public TMP_InputField _email;
        public TMP_InputField _password;
        public Text _warningText;

        public string scene;
        public string displayName;
        private string key = "NexusConnects";

        string url = "http://127.0.0.1:5000/authenticate_with_unity";

        //private void Start()
        //{
        //    _warningText.text = "";
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
            StartCoroutine(SendPostCoroutine());
        }

        IEnumerator SendPostCoroutine()
        {
            //StartGame();
            //Debug.Log("login activated with " + _email.text + _password.text);
            //string jsonString = "{\"email\":\"" + _email.text + "\",\"password\":\"" + _password.text + "\",\"key\":\"" + key + "\"}";
            //Debug.Log(jsonString);
            ////CoroutineWithData cd = new CoroutineWithData(this, Post("http://127.0.0.1:5000/authenticate_with_unity", jsonString));
            ////yield return cd.coroutine;
            ////StartCoroutine(Post("http://127.0.0.1:5000/authenticate_with_unity", jsonString));
            ////Debug.Log("result is " + cd.result);  //  'success' or 'fail'
            ///

            WWWForm form = new WWWForm();
            form.AddField("email", _email.text);
            form.AddField("password", _password.text);
            form.AddField("key", key);

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("POST successful!");
                    StringBuilder sb = new StringBuilder();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                    {
                        sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                    }

                    // Print Headers
                    Debug.Log(sb.ToString());

                    // Print Body
                    Debug.Log(www.downloadHandler.text);
                    LoginInfo info = JsonUtility.FromJson<LoginInfo>(www.downloadHandler.text);
                    Debug.Log(info.code.GetType());
                    Debug.Log(info.code);
                    if(info.code == 0)
                    {
                        Debug.Log("Success!");
                        Debug.Log(info.name);
                        Debug.Log(info.admin);
                        StartGame(info.name, info.admin);
                    }
                    else if (info.code == 403)
                    {
                        Debug.Log("wrong pass");
                        _warningText.text = "Are you sure you put in the right password? Check the website.";
                    }
                    else if(info.code == 404)
                    {
                        Debug.Log("not found");
                        _warningText.text = "User not found, make sure you put in the right email";
                    }
                }
            }


        }

        public void StartGame(string name, string isAdmin)
        {
            Debug.Log("StartGame() Called!");
            PlayerPrefs.SetString("playerName", name);
            PlayerPrefs.SetString("isAdmin", isAdmin);

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
