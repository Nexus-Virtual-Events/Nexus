// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Normal.Realtime;
//
// public class AvatarSync : RealtimeComponent {
//
//     private ThirdPersonOrbitCamBasic _orbitCam;
//     private AvatarSyncModel _model;
//
//     private void Start() {
//         // Get a reference to the mesh renderer
//         _orbitCam = GetComponent<ThirdPersonOrbitCamBasic>();
//     }
//
//     private AvatarSyncModel model {
//         set {
//             // Store the model
//             _model = value;
//         }
//     }
//
//     private void UpdateAvatar() {
//         // Get the color from the model and set it on the mesh renderer.
//         _orbitCam.text = _model.avatar;
//     }
// }
