using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Normal.Realtime;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using TMPro;

namespace Michsky.UI.ModernUIPack {
    public class AvatarManager : MonoBehaviour {
        private Realtime _realtime;

        public ModalWindowManager welcomeWindow;
        public ModalWindowManager settingsWindow;
        public ModalWindowManager adminPanelWindow;
        public NotificationManager connectedNotification;
        public NotificationManager eventStartingNotification;
        private EventManager eventManager;
        private GameObject localPlayer;

        public GameObject trees;

        //public AudioMixer audioMixer;

        public ResolutionSelector resolutionSelector;
        Resolution[] resolutions;
        UnityEvent[] onResolutionChanges;

        public GameObject MainCamera;
        public GameObject FallBackCamera;

        public GameObject ReconnectUI;

        private bool isConnected;

        public TMP_Text roomText;
        public Image roomImage;

        public List<Sprite> roomSprites;

        private void Awake() {
            // Get the Realtime component on this game object
            _realtime = GetComponent<Realtime>();

            // Notify us when Realtime successfully connects to the room
            _realtime.didConnectToRoom += DidConnectToRoom;
            _realtime.didDisconnectFromRoom += DidDisconnectFromRoom;

            string roomIndex = PlayerPrefs.GetString("roomName");
            _realtime._roomToJoinOnStart = roomIndex;

            if(roomIndex == "Room1")
            {
                roomText.text = "Azure Akita";
                roomImage.sprite = roomSprites[0];
            }
            else if(roomIndex == "Room2")
            {
                roomText.text = "Crimson Koala";
                roomImage.sprite = roomSprites[1];
            }
            else if (roomIndex == "Room3")
            {
                roomText.text = "Golden Grizzly";
                roomImage.sprite = roomSprites[2];
            }
            else if(roomIndex == "Room4")
            {
                roomText.text = "Ivory Ibex";
                roomImage.sprite = roomSprites[3];
            }
            else if (roomIndex == "Room5")
            {
                roomText.text = "Jade Jackal";
                roomImage.sprite = roomSprites[4];
            }
            else
            {
                // roomText.text = "Administrator";
                // roomImage.sprite = roomSprites[5];
            }


            _spawn = GameObject.Find("Spawn").transform;



        }

        //public void ConnectToRoom() {
        //    _realtime.Connect("The Circle");
        //}

        public void ForceDisconnect () {
            _realtime.Disconnect();
        }
        private void Start()
        {
            eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.OnEventsChange.AddListener(ReactToEvent);

            resolutions = Screen.resolutions;
            ResolutionSelector.resolutions = resolutions;
            List<ResolutionSelector.Item> resolutionOptions = new List<ResolutionSelector.Item>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width.ToString() + " x " + resolutions[i].height.ToString();
                ResolutionSelector.Item item = new ResolutionSelector.Item(option);
                resolutionOptions.Add(item);

                if (resolutions[i].width == Screen.currentResolution.width
                    && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionSelector.itemList = resolutionOptions;
            resolutionSelector.defaultIndex = currentResolutionIndex;
            resolutionSelector.index = currentResolutionIndex;
        }

        private bool DidConnect;

        private Transform _spawn;

       
        private void DidConnectToRoom(Realtime realtime) {

            Debug.Log("Connected from AvatarManager as " + _realtime._roomToJoinOnStart);

            MainCamera.SetActive(true);
            FallBackCamera.SetActive(false);
            ReconnectUI.SetActive(false);
            // Instantiate the CubePlayer for this client once we've successfully connected to the room
            localPlayer = Realtime.Instantiate("NexusAvatar",                 // Prefab name
                                position: _spawn.position,          // Start 1 meter in the air
                                rotation: _spawn.rotation, // No rotation
                           ownedByClient: true,                // Make sure the RealtimeView on this prefab is owned by this client
                preventOwnershipTakeover: true,                // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                             useInstance: realtime);           // Use the instance of Realtime that fired the didConnectToRoom event.
            Debug.Log(">>>> ROOM NAME: " + _realtime._roomToJoinOnStart);

            localPlayer.layer = LayerMask.NameToLayer(_realtime._roomToJoinOnStart);
            Debug.Log(">> Joined from Avatar Manager and assigned to layer "+ LayerMask.NameToLayer(_realtime._roomToJoinOnStart));
            Utils.localPlayers.Add(localPlayer);
            ShowWelcomeWindow();
            if(PlayerPrefs.GetString("adminRoom") == "true"){
                localPlayer.layer = LayerMask.NameToLayer("Hidden");
            }

            SendConnectionInfo("true");

            nmrLoadingReconnectTrial = 0;
        }

       


        private int nmrReconnectTrial = 0;
        private float lastReconnectTrial = 0;

        private float startedConnecting = -1;

        private int nmrLoadingReconnectTrial = 0;

        void Update () {
            
                if (nmrReconnectTrial >= 5) {
                    Debug.Log("Not trying again");
                    return;
                }

                if (_realtime.disconnected && !_realtime.connecting && Time.time > lastReconnectTrial + 3 * nmrReconnectTrial) {
                    nmrReconnectTrial += 1;

                    lastReconnectTrial = Time.time;
                    Debug.Log("TRYING TO CONNECT");
                    _realtime.Connect("The Circle");
                }

                if(nmrLoadingReconnectTrial > 10){
                    Application.Quit();
                }

                if(_realtime.connecting){
                    if (startedConnecting < 0) {
                        // Debug.Log("Started CONNECTING");
                        startedConnecting = Time.time;
                        nmrLoadingReconnectTrial += 1;
                    }

                    if (Time.time > startedConnecting + 5.0f * nmrLoadingReconnectTrial) {
                        // Debug.Log("FORCE DISCONNECT");
                        startedConnecting = -1;
                        ForceDisconnect();
                    }
                }
                
            }
        private void DidDisconnectFromRoom(Realtime realtime){
            
            if(MainCamera){
                MainCamera.transform.parent = null;
                MainCamera.SetActive(false);
            }
            if(FallBackCamera){
            FallBackCamera.SetActive(true);
            }
            if(ReconnectUI){
            ReconnectUI.SetActive(true);
            }

            nmrReconnectTrial = 0;
            lastReconnectTrial = 0;
            startedConnecting = -1;

            FallBackCamera.transform.position = MainCamera.transform.position;
            FallBackCamera.transform.rotation = MainCamera.transform.rotation;
            
            if(localPlayer){
                _spawn.position = localPlayer.transform.position;
                _spawn.rotation = localPlayer.transform.rotation;
            }

            Destroy(localPlayer);

            SendConnectionInfo("false");

        }

        public void ShowWelcomeWindow()
        {
            welcomeWindow.OpenWindow();
        }

        public void ShowConnectedNotification()
        {
            connectedNotification.OpenNotification();
        }

        public void ReactToEvent()
        {
            if (eventManager.GetEvents() == null) return;
            if (eventManager.GetEvents()[1] == '1')
            {
                eventStartingNotification.OpenNotification();
            }
            else if (eventManager.GetEvents()[1] == '0')
            {
                eventStartingNotification.CloseNotification();
            }
        }

        public void ShowSettingsWindow()
        {
            settingsWindow.OpenWindow();
        }

        public void ShowAdminPanelWindow()
        {
            adminPanelWindow.OpenWindow();
        }

        public void SetGraphicsSetting(int settingIndex)
        {
            Debug.Log(settingIndex);
            QualitySettings.SetQualityLevel(settingIndex);
            if(settingIndex >= 2)
            {
                ToggleTrees();
            }
        }

        public void SetVolume(float volume)
        {
            //audioMixer.SetFloat("Volume", volume);
        }

        public void ToggleTrees()
        {
            trees.SetActive(!trees.activeSelf);
        }

        public void Respawn()
        {
            // Realtime.Destroy(localPlayer.GetComponent<RealtimeView>());
            _realtime.Disconnect();
            // Destroy(localPlayer);
            //GameObject.Find("Realtime").GetComponent<Realtime>().Disconnect();
            Loading.sceneString = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Loading");
        }

        public void QuitGame()
        {
            // Realtime.Destroy(localPlayer.GetComponent<RealtimeView>());
            _realtime.Disconnect();
            // Destroy(localPlayer);
            //GameObject.Find("Realtime").GetComponent<Realtime>().Disconnect();
            Application.Quit();
        }

        string url = "http://127.0.0.1:5000/change_room_count";
    
        public void SendConnectionInfo(string isConnecting)
        {
            StartCoroutine(SendPostCoroutine(isConnecting));
        }

        IEnumerator SendPostCoroutine(string isConnecting)
        {
            WWWForm form = new WWWForm();
            form.AddField("room", _realtime._roomToJoinOnStart);
            form.AddField("isConnecting", isConnecting);

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

                    // Print Body
                    LoginInfo info = JsonUtility.FromJson<LoginInfo>(www.downloadHandler.text);
                    if(info.code == 0)
                    {
                        Debug.Log("Success!");
                    }
                    else if(info.code == 404)
                    {
                        Debug.Log("not found");
                    }
                }
            }


        }
    }
}
