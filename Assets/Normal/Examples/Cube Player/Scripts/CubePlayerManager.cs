using UnityEngine;
using Normal.Realtime;

namespace Michsky.UI.ModernUIPack {
    public class CubePlayerManager : MonoBehaviour {
        private Realtime _realtime;

        public ModalWindowManager welcomeWindow;
        public ModalWindowManager settingsWindow;
        public NotificationManager connectedNotification;
        public NotificationManager eventStartingNotification;
        public GameObject grassTerrain;

        private void Awake() {
            // Get the Realtime component on this game object
            _realtime = GetComponent<Realtime>();

            // Notify us when Realtime successfully connects to the room
            _realtime.didConnectToRoom += DidConnectToRoom;
        }

        private void DidConnectToRoom(Realtime realtime) {
            // Instantiate the CubePlayer for this client once we've successfully connected to the room
            Realtime.Instantiate("CubePlayer",                 // Prefab name
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

        public void ShowEventStartingNotification()
        {
            eventStartingNotification.OpenNotification();
        }

        public void ShowSettingsWindow()
        {
            settingsWindow.OpenWindow();
        }

        public void SetGraphicsSetting(int settingIndex)
        {
            QualitySettings.SetQualityLevel(settingIndex);
            if (settingIndex == 0)
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
    }
}
