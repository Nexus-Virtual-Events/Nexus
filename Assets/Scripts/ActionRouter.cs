using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using agora_gaming_rtc;
using agora_utilities;
using UnityEngine.Playables;

public class ActionRouter : MonoBehaviour
{
    private static GameObject localAvatar;
    private static GameObject currentChair;
    private static GameObject currentCharacter;
    private AudioRecordingDeviceManager audioRecordingDeviceManager;

    // Start is called before the first frame update
    void Start()
    {
        audioRecordingDeviceManager = (AudioRecordingDeviceManager)AgoraMainMenu.app.mRtcEngine.GetAudioRecordingDeviceManager();
    }

    private bool isMuted = false;
    public GameObject highlightedMic;
    public void ToggleMute(){
        AgoraMainMenu.app.mRtcEngine.MuteLocalAudioStream(!isMuted);
        AgoraMainMenu.app.mRtcEngine.EnableLocalAudio(isMuted);
        isMuted = !isMuted;
        Debug.Log("isMuted " + isMuted.ToString());
        highlightedMic.GetComponent<CanvasGroup>().alpha = Convert.ToSingle(isMuted);
    }

    private bool isCameraOff = false;
    public GameObject highlightedCam;

    public void ToggleCamera(){
        AgoraMainMenu.app.mRtcEngine.MuteLocalVideoStream(!isCameraOff);
        isCameraOff = !isCameraOff;
        Debug.Log("isCameraOff " + isCameraOff.ToString());
        if(isCameraOff){
            AgoraMainMenu.app.mRtcEngine.DisableVideoObserver();
        }
        else{
            AgoraMainMenu.app.mRtcEngine.EnableVideoObserver();
        }
        highlightedCam.GetComponent<CanvasGroup>().alpha = Convert.ToSingle(isCameraOff);
    }

    public GameObject highlightedSound;
    private bool areSoundsOff = false;
    public void ToggleSounds(){
        areSoundsOff = !areSoundsOff;
        AudioListener.volume = Convert.ToSingle(areSoundsOff);
        highlightedSound.GetComponent<CanvasGroup>().alpha = Convert.ToSingle(areSoundsOff);
    }

    // public GameObject highlightedGlobal;
    // public void ToggleGlobal(bool b){
    //    highlightedGlobal.GetComponent<CanvasGroup>().alpha = Convert.ToSingle(b);
    // }

    // Update is called once per frame
    void Update()
    {
    
    }

    public static void SetCurrentChair(GameObject o)
    {
        currentChair = o;
    }

    public static void SetLocalAvatar(GameObject o)
    {
        localAvatar = o;
    }

    public static void SetCurrentCharacter(GameObject o)
    {
        currentCharacter = o;
    }

    public static GameObject GetLocalAvatar()
    {
        return localAvatar;
    }

    public static GameObject GetCurrentChair()
    {
        return currentChair;
    }

    public static GameObject GetCurrentCharacter()
    {
        return currentCharacter;
    }
    // ActionRouter.SetLocalAvatar(....);
}


