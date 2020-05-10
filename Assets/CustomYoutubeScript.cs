using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using TMPro;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI;

public class CustomYoutubeScript : MonoBehaviour
{
    private YoutubePlayer player;

    public string url;
    public bool autoPlay = true;
    private VideoPlayer videoPlayer;

    private int currentTime;
    private int enabled;
    private int fullscreen;
    private int isPaused;
    private float volume;
    private string youtubeUrl;

    private YoutubeSync youtubeSync;

    public GameObject linkInputObject;
    private TMP_InputField linkInput;

    private bool urlInControl;

    public GameObject screen;

    public GameObject enableButtonText;

    private int prevEnabled;
    private int prevPaused;

    public GameObject volumeSlider;

    public GameObject myVideoPlayer;

    public GameObject timeTextObject;
    public GameObject videoTimeSlider;


    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        player = GetComponentInChildren<YoutubePlayer>();
        player.videoPlayer = videoPlayer;

        currentTime = 0;
        enabled = 0;
        fullscreen = 0;
        isPaused = 0;
        volume = 0.5f;
        youtubeUrl = "";
    }

    private void Start()
    {
        urlInControl = false;
        
        youtubeSync = GetComponent<YoutubeSync>();
        InvokeRepeating("UpdateModel", 0, 0.5f);
        InvokeRepeating("UpdateScreen", 0, 0.5f);

        Debug.Log("object:"+linkInputObject.ToString());
        // linkInputObject = GameObject.Find("LinkInput");
        linkInput = linkInputObject.GetComponent<TMP_InputField>();
        // PlayNew(Convert.ToInt32(youtubeSync.GetYoutubeParameter(3)));
        prevEnabled = Convert.ToInt32(youtubeSync.GetYoutubeParameter(0));
        prevPaused = Convert.ToInt32(youtubeSync.GetYoutubeParameter(2));

        volume = float.Parse(youtubeSync.GetYoutubeParameter(4));
    }

    private string FormatTime(int time)
    {
        if(time < 0){
            return "00:00";
        }
        int hours = time / 3600;
        int minutes = (time % 3600) / 60;
        int seconds = (time % 3600) % 60;
        if (hours == 0 && minutes != 0)
        {
            return minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else if (hours == 0 && minutes == 0)
        {
            return "00:" + seconds.ToString("00");
        }
        else
        {
            return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    public void SetNewUrl(){
        Debug.Log("url update");
        youtubeUrl = linkInput.text;
        currentTime = 0;
        fullscreen = 0;
        isPaused = 0;
    }

    public void ToggleEnableScreen(){
        enabled = 1 - enabled;
        Debug.Log(enabled);
    }

    private void UpdateModel(){
        youtubeSync.SetYoutube(YoutubeToString(enabled, fullscreen, isPaused, player.GetCurrentTime(), volume, youtubeUrl));
    }

    public void ChangeVolume(float f){
        volume = f;
    }

    private string prevUrl = "";
    private void UpdateScreen(){
        if(youtubeUrl != prevUrl){
            PlayNew(youtubeUrl, 0);
            prevUrl = youtubeUrl;
        }
        if(enabled != prevEnabled){
            Debug.Log("switch from model!");
            if(enabled == 0){
                enableButtonText.GetComponent<TMP_Text>().text = "OFF";
                screen.GetComponent<MeshRenderer>().enabled = false;
            }
            else{
                enableButtonText.GetComponent<TMP_Text>().text= "ON";
                screen.GetComponent<MeshRenderer>().enabled = true;
            }
            prevEnabled = enabled;
        }
        if(isPaused != prevPaused){
            Debug.Log("switch from model!");
            if(isPaused == 0){
                PlayNew(youtubeUrl, player.GetCurrentTime());
                // enableButtonText.GetComponent<TMP_Text>().text = "OFF";
                // screen.GetComponent<MeshRenderer>().enabled = false;
            }
            else{
                player.Pause();
                // enableButtonText.GetComponent<TMP_Text>().text= "ON";
                // screen.GetComponent<MeshRenderer>().enabled = true;
            }
            prevPaused = isPaused;
        }

        volumeSlider.GetComponent<Slider>().value = volume;
        myVideoPlayer.GetComponent<AudioSource>().volume = volume;
        
        timeTextObject.GetComponent<TMP_Text>().text = FormatTime(player.GetCurrentTime());
        Debug.Log("current time:" + player.GetCurrentTime());
        Debug.Log("total time:" + player.GetTotalDuration());
        Debug.Log(100f * ((float)player.GetCurrentTime()/(float)player.GetTotalDuration()));

        videoTimeSlider.GetComponent<Slider>().value = 100f * ((float)player.GetCurrentTime()/(float)player.GetTotalDuration());

    }

    public void ReceiveUpdate(int _enabled, int _fullscreen , int _isPaused, int _currentTime, float _volume, string _youtubeUrl){
        enabled = _enabled;
        currentTime = _currentTime;
        isPaused = _isPaused;
        volume = _volume;
        youtubeUrl = _youtubeUrl;
        fullscreen = _fullscreen;
    }

    public void UrlInControl(){
        Debug.Log("In control");
        urlInControl = true;
    }

    public void UrlOutControl(){
        Debug.Log("Out control");
        urlInControl = false;
    }

    private void Update(){
        
    }


    private string YoutubeToString(int _enabled, int _fullscreen , int _isPaused, int _currentTime, float _volume, string _youtubeUrl){
        return _enabled.ToString() + "_" + _fullscreen.ToString() + "_" + _isPaused.ToString() + "_" + _currentTime.ToString() 
        + "_" + _volume.ToString() + "_" + _youtubeUrl.ToString(); 
    }

    public void PlayNew(string url, int time){
        player.Play(url);
        player.Seek(time);
    }

    // public void Play(int time)
    // {   
        
    //     player.Play(url);
    //     player.Seek(time);
        // if (fullscreen)
        // {
        //     videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        // }
        // player.autoPlayOnStart = autoPlay;
        // player.videoQuality = YoutubePlayer.YoutubeVideoQuality.STANDARD;


        // if(autoPlay)
        //     player.Play(url);
    // }

    // public void StartVideo(){
    //     player.Start();
    // }

    // public void PauseVideo(){
    //     player.Pause();
    // }

    public void TogglePause(){
        isPaused = 1 - isPaused;
    }
}
