using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Normal.Realtime;
using UnityEditor;

namespace Michsky.UI.ModernUIPack {
    public class SimpleAvatarManager : MonoBehaviour {
        private Realtime _realtime;

        private GameObject localPlayer;

        //public AudioMixer audioMixer;

        public GameObject MainCamera;
        public GameObject FallBackCamera;

        public GameObject ReconnectUI;

        private void Awake() {
            // Get the Realtime component on this game object
            _realtime = GetComponent<Realtime>();

            // Notify us when Realtime successfully connects to the room
            _realtime.didConnectToRoom += DidConnectToRoom;
            _realtime.didDisconnectFromRoom += DidDisconnectFromRoom;
            // _realtime._roomToJoinOnStart = PlayerPrefs.GetString("roomName");

            _spawn = GameObject.Find("Spawn").transform;

        }

        //public void ConnectToRoom() {
        //    _realtime.Connect("The Circle");
        //}

        public void ForceDisconnect () {
            _realtime.Disconnect();
        }

        private bool DidConnect;

        private Transform _spawn;
        private void DidConnectToRoom(Realtime realtime) {
            Debug.Log("Someone else connected from " + _realtime._roomToJoinOnStart);
            // Debug.Log("DID CONNECT");
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
            localPlayer.layer = LayerMask.NameToLayer(_realtime._roomToJoinOnStart);
            Utils.localPlayers.Add(localPlayer);
            ShowWelcomeWindow();
        
            nmrLoadingReconnectTrial = 0;

            InvokeRepeating("BringAllTransforms", 15.0f, 2.0f);

        }

         private void BringAllTransforms(){
            if(localPlayer != null && LayerMask.LayerToName(localPlayer.layer) == SceneRoomRouter.currentLayer){
                Transform localTransform = localPlayer.transform; 
                foreach(GameObject local in Utils.localPlayers){
                    local.transform.position = localTransform.position;
                    local.transform.rotation = localTransform.rotation;
                    }
            }
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

        }

        public void ShowWelcomeWindow()
        {
            // welcomeWindow.OpenWindow();
        }

        public void ShowConnectedNotification()
        {
            // connectedNotification.OpenNotification();
        }

        public void ReactToEvent()
        {
            // if (eventManager.GetEvents() == null) return;
            // if (eventManager.GetEvents()[1] == '1')
            // {
            //     eventStartingNotification.OpenNotification();
            // }
            // else if (eventManager.GetEvents()[1] == '0')
            // {
            //     eventStartingNotification.CloseNotification();
            // }
        }

        public void ShowSettingsWindow()
        {
            // settingsWindow.OpenWindow();
        }

        public void ShowAdminPanelWindow()
        {
            // adminPanelWindow.OpenWindow();
        }

        public void SetGraphicsSetting(int settingIndex)
        {
            // Debug.Log(settingIndex);
            // QualitySettings.SetQualityLevel(settingIndex);
            // if(settingIndex >= 2)
            // {
            //     ToggleTrees();
            // }
        }

        public void SetVolume(float volume)
        {
            //audioMixer.SetFloat("Volume", volume);
        }

        public void ToggleTrees()
        {
            // trees.SetActive(!trees.activeSelf);
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
    }
}
