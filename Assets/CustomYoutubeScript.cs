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

    private int pauseTime;

    private string currentTimeStamp;
    private string prevTimeStamp;



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
        InvokeRepeating("UpdateModel", 0, 0.1f);
        InvokeRepeating("UpdateScreen", 0, 0.1f);

        Debug.Log("object:"+linkInputObject.ToString());
        // linkInputObject = GameObject.Find("LinkInput");
        linkInput = linkInputObject.GetComponent<TMP_InputField>();
        // PlayNew(Convert.ToInt32(youtubeSync.GetYoutubeParameter(3)));
        prevEnabled = Convert.ToInt32(youtubeSync.GetYoutubeParameter(0));
        prevPaused = Convert.ToInt32(youtubeSync.GetYoutubeParameter(2));

        if(prevPaused == 1){
            pauseTime = prevPaused;
        }

        volume = float.Parse(youtubeSync.GetYoutubeParameter(4));
        currentTimeStamp = youtubeSync.GetYoutubeParameter(6);
        prevTimeStamp = currentTimeStamp;
        currentTime = Convert.ToInt32(youtubeSync.GetYoutubeParameter(3));
    }

    private void Update(){
        // UpdateModel();
        // UpdateScreen();
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
        youtubeSync.SetYoutube(YoutubeToString(enabled, fullscreen, isPaused, currentTime, volume, youtubeUrl, currentTimeStamp));
    }

    public void ChangeVolume(float f){
        volume = f;
    }

    private string prevUrl = "";

    private bool playerEdit;
    private void UpdateScreen(){
        if(youtubeUrl != prevUrl){
            PlayNew(youtubeUrl, currentTime);
            prevUrl = youtubeUrl;
        }
        if(enabled != prevEnabled){
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
            if(isPaused == 0){
                StartVideo();
                // enableButtonText.GetComponent<TMP_Text>().text = "OFF";
                // screen.GetComponent<MeshRenderer>().enabled = false;
            }
            else{
                PauseVideo();
                // enableButtonText.GetComponent<TMP_Text>().text= "ON";
                // screen.GetComponent<MeshRenderer>().enabled = true;
            }
            prevPaused = isPaused;
        }

        if(currentTimeStamp != prevTimeStamp && playerEdit){
                    // Debug.Log("different timestamp: seeking");
                    player.Seek(currentTime);
                    player.Play();
                    prevTimeStamp = currentTimeStamp;
                }

        if(!playerEdit){
            currentTime = player.GetCurrentTime();
            videoTimeSlider.GetComponent<Slider>().value = 100f * ((float)player.GetCurrentTime()/(float)player.GetTotalDuration());
        }

        volumeSlider.GetComponent<Slider>().value = volume;
        myVideoPlayer.GetComponent<AudioSource>().volume = volume;
        
        timeTextObject.GetComponent<TMP_Text>().text = FormatTime(player.GetCurrentTime());

    }

    public void ReceiveUpdate(int _enabled, int _fullscreen , int _isPaused, int _currentTime, float _volume, string _youtubeUrl, string timeStamp){
        enabled = _enabled;
        currentTime = _currentTime;
        isPaused = _isPaused;
        volume = _volume;
        youtubeUrl = _youtubeUrl;
        fullscreen = _fullscreen;
        currentTimeStamp = timeStamp;
    }

    public void UrlInControl(){
        Debug.Log("In control");
        urlInControl = true;
    }

    public void UrlOutControl(){
        Debug.Log("Out control");
        urlInControl = false;
    }


    private string YoutubeToString(int _enabled, int _fullscreen , int _isPaused, int _currentTime, float _volume, string _youtubeUrl, string _timeStamp){
        return _enabled.ToString() + "_" + _fullscreen.ToString() + "_" + _isPaused.ToString() + "_" + _currentTime.ToString() 
        + "_" + _volume.ToString() + "_" + _youtubeUrl + "_" + _timeStamp; 
    }

    public void PlayNew(string url, int time){
        player.Play(url);
        player.Seek(time);
    }

    public void PlayerEdit(){
        playerEdit = true;
    }

    public void PlayerUnedit(){
        playerEdit = false;
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
    private bool changedTime;
    public void ChangeTime(float f){
        if(playerEdit){
            currentTime = (int)(f*(float)player.GetTotalDuration()/100f);
            // Debug.Log("currentTime from change" + currentTime.ToString());
            currentTimeStamp = Math.Round((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
            // youtubeSync.SetYoutube(YoutubeToString(enabled, fullscreen, isPaused, player.GetCurrentTime(), volume, youtubeUrl, currentTimeStamp));
        }
    }

    public void StartVideo(){
        Debug.Log("video start");
        player.Seek(pauseTime);
        player.Play();
    }

    public void AddFive(){
        if(player.GetCurrentTime() + 5 < player.GetTotalDuration()){
            player.Seek(player.GetCurrentTime() + 5);
            player.Play();
            return;
        }
        Debug.Log("add5");
    }

    public void SubtractFive(){
        if(player.GetCurrentTime() - 5 > 0){
            player.Seek(player.GetCurrentTime() - 5);
            player.Play();
            return;
        }
    }

    public void PauseVideo(){
        Debug.Log("video start");
        player.Pause();
        pauseTime = player.GetCurrentTime();
    }

    public void TogglePause(){
        isPaused = 1 - isPaused;
    }
}
