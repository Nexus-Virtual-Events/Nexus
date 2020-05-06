using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Normal.Realtime;
using UnityEditor;

namespace Michsky.UI.ModernUIPack {
    public class AvatarManager : MonoBehaviour {
        private Realtime _realtime;

        public ModalWindowManager welcomeWindow;
        public ModalWindowManager settingsWindow;
        public NotificationManager connectedNotification;
        public NotificationManager eventStartingNotification;
        private EventManager eventManager;
        private GameObject localPlayer;

        public GameObject trees;

        //public AudioMixer audioMixer;

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
            Debug.Log(resolutionSelector.itemList);
            resolutionSelector.defaultIndex = currentResolutionIndex;
            resolutionSelector.index = currentResolutionIndex;
        }

        private void DidConnectToRoom(Realtime realtime) {
            // Instantiate the CubePlayer for this client once we've successfully connected to the room
            localPlayer = Realtime.Instantiate("NexusAvatar",                 // Prefab name
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
            Realtime.Destroy(localPlayer.GetComponent<RealtimeView>());
            Destroy(localPlayer);
            //GameObject.Find("Realtime").GetComponent<Realtime>().Disconnect();
            Loading.sceneString = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Loading");
        }

        public void QuitGame()
        {
            Realtime.Destroy(localPlayer.GetComponent<RealtimeView>());
            Destroy(localPlayer);
            //GameObject.Find("Realtime").GetComponent<Realtime>().Disconnect();
            Application.Quit();
        }
    }
}
