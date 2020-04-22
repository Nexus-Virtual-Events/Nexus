using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Normal.Realtime;
using System.Linq;

namespace Michsky.UI.ModernUIPack {
    public class CubePlayerManager : MonoBehaviour {
        private Realtime _realtime;

        public ModalWindowManager welcomeWindow;
        public ModalWindowManager settingsWindow;
        public NotificationManager connectedNotification;
        public NotificationManager eventStartingNotification;
        public GameObject grassTerrain;
        private EventManager eventManager;
        private GameObject localPlayer;

        public AudioMixer audioMixer;

        public ResolutionSelector resolutionSelector;
        Resolution[] resolutions;
        UnityEvent[] onResolutionChanges;


        private void Awake() {
            // Get the Realtime component on this game object
            _realtime = GetComponent<Realtime>();

            // Notify us when Realtime successfully connects to the room
            _realtime.didConnectToRoom += DidConnectToRoom;

        }

        private void Start()
        {
            eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.OnEventsChange.AddListener(ReactToEvent);

            resolutions = Screen.resolutions;
            ResolutionSelector.resolutions = resolutions;
            //resolutionSelector.itemList = null;
            List<ResolutionSelector.Item> resolutionOptions = new List<ResolutionSelector.Item>();

            //onResolutionChanges = new UnityEvent[resolutions.Length];
            //for (int i = 0; i < resolutions.Length; i++)
            //{
            //    Debug.Log("resolution indexes: " + i.ToString());
            //    onResolutionChanges[i] = new UnityEvent();
            //    onResolutionChanges[i].AddListener(delegate { SetResolution(i); });
            //    Debug.Log("onResolutionChanges[i] = :");
            //}

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width.ToString() + " x " + resolutions[i].height.ToString();
                Debug.Log("Resolution " + i.ToString() + ": " + option);
                ResolutionSelector.Item item = new ResolutionSelector.Item(option);
                resolutionOptions.Add(item);

                if (resolutions[i].width == Screen.currentResolution.width
                    && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionSelector.itemList = resolutionOptions;
            Debug.Log(resolutionSelector.itemList);
            resolutionSelector.defaultIndex = currentResolutionIndex;
            resolutionSelector.index = currentResolutionIndex;
        }

        private void DidConnectToRoom(Realtime realtime) {
            // Instantiate the CubePlayer for this client once we've successfully connected to the room
            localPlayer = Realtime.Instantiate("CubePlayer",                 // Prefab name
                                position: Vector3.up,          // Start 1 meter in the air
                                rotation: Quaternion.identity, // No rotation
                           ownedByClient: true,                // Make sure the RealtimeView on this prefab is owned by this client
                preventOwnershipTakeover: true,                // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                             useInstance: realtime);           // Use the instance of Realtime that fired the didConnectToRoom event.
            ShowWelcomeWindow();
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

        public void SetGraphicsSetting(int settingIndex)
        {
            Debug.Log(settingIndex);
            QualitySettings.SetQualityLevel(settingIndex);
            if (settingIndex < 3)
            {
                grassTerrain.GetComponent<Terrain>().enabled = false;
            }
            else
            {
                grassTerrain.GetComponent<Terrain>().enabled = true;
            }
        }

        //public void SetResolution(int resolutionIndex)
        //{
        //    Debug.Log("Setting Resolution to " + resolutionIndex.ToString());

        //    Resolution resolution = resolutions[resolutionIndex-1];
        //    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        //}

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("Volume", volume);
        }

        public void ToggleGrass()
        {
            grassTerrain.GetComponent<Terrain>().enabled = !grassTerrain.GetComponent<Terrain>().enabled;
        }

        public void Respawn()
        {
            GameObject.Find("Realtime").GetComponent<Realtime>().Disconnect();
            Loading.sceneString = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Loading");
        }
    }
}
