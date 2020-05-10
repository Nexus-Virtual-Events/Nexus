using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;

public class YoutubeSync : RealtimeComponent {

    private CustomYoutubeScript   _youtubeScript;
    private YoutubeSyncModel _model;

    private void Start() {
        // Get a reference to the mesh renderer
        _youtubeScript = GetComponent<CustomYoutubeScript>();
        
    }

    private YoutubeSyncModel model {
        set {
            if (_model != null) {
                // Unregister from events
                _model.youtubeDidChange -= YoutubeDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null) {
                // Update the mesh render to match the new model
                UpdateFromModel();

                // Register for events so we'll know if the color changes later
                _model.youtubeDidChange += YoutubeDidChange;
            }
        }
    }

    private void YoutubeDidChange(YoutubeSyncModel model, string value) {
        // Update the mesh renderer
        UpdateFromModel();
    }

    public string GetYoutubeParameter(int parameter){
        if(_model.youtube == null || _model.youtube == ""){
            return "";
        }
        return _model.youtube.Split('_')[parameter];
    }

   
    private void UpdateFromModel() {
        // Get the color from the model and set it on the mesh renderer.
        if(_model.youtube == null || _model.youtube == ""){
            return;
        }
       string[] parameters = _model.youtube.Split('_');
       int enabled = Convert.ToInt32(parameters[0]);
       int fullscreen = Convert.ToInt32(parameters[1]);
       int isPaused = Convert.ToInt32(parameters[2]);
       int currentTime = Convert.ToInt32(parameters[3]);
       float volume = float.Parse(parameters[4]);
       string youtubeUrl = parameters[5];

       _youtubeScript.ReceiveUpdate(enabled, fullscreen, isPaused, currentTime, volume, youtubeUrl);
    }

    public void SetYoutube(string youtube) {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        _model.youtube = youtube;
    }
}
