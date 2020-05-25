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
        public string isStudent;

    }

    public class MainMenu : MonoBehaviour
    {
        public TMP_InputField _email;
        public TMP_InputField _password;
        public TMP_Text _warningText;

        public string scene;
        public string displayName;
        private string key = "NexusConnects";

        string url = "http://the-nexus.herokuapp.com/authenticate_with_unity";
    
        public void Login()
        {
            StartCoroutine(SendPostCoroutine());
        }

        IEnumerator SendPostCoroutine()
        {
            WWWForm form = new WWWForm();
            form.AddField("email", _email.text);
            form.AddField("password", _password.text);
            form.AddField("key", key);

            Debug.Log(_email.text);
            Debug.Log(_password.text);

            _warningText.text = "Loading...";
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    Debug.Log(www.error);
                    _warningText.text = "There seems to be a network error";
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
                        StartGame(info.name, info.admin, info.isStudent);
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

        public void StartGame(string name, string isAdmin, string isStudent)
        {
            Debug.Log("StartGame() Called!");
            PlayerPrefs.SetString("playerName", name);
            PlayerPrefs.SetString("isAdmin", isAdmin);
            PlayerPrefs.SetString("isStudent", isStudent);


            RoomRouter.sceneString = scene;
            Loading.sceneString = "Room Router";
            SceneManager.LoadScene("Loading");
        }
    }
}
