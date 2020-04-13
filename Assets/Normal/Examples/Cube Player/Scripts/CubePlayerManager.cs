using UnityEngine;
using UnityEngine.SceneManagement;
using Normal.Realtime;

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
            if (eventManager.events == null) return;
            if (eventManager.events[1] == '1')
            {
                eventStartingNotification.OpenNotification();
            }
            else if (eventManager.events[1] == '0')
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
