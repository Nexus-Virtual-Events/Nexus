using UnityEngine;
using Normal.Realtime;

// namespace Normal.Realtime.Examples {
    public class ThirdPersonPlayer : MonoBehaviour {
        public float speed = 5.0f;

        private RealtimeView      _realtimeView;
        private RealtimeTransform _realtimeTransform;

        private void Awake() {

            _realtimeView      = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        private void Update() {
            // If this CubePlayer prefab is not owned by this client, bail.
            if (!_realtimeView.isOwnedLocally)
                return;

            // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
            _realtimeTransform.RequestOwnership();

            // GUI Here
        }
    }
// }
