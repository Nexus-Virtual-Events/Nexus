
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using Michsky.UI.ModernUIPack;


public class Utils : MonoBehaviour
{
    public static Map<int, string> interactionMap;
    public static Dictionary<int, float[]> animationRequirements;
    public static Dictionary<string, string> sceneNames;
    public static string[] animations;
    public static UnityEngine.KeyCode[] animationEnums;

    public GameObject adminWindow;

    public void MoveToRoom(string roomName)
    {
        Loading.sceneString = roomName;
        SceneManager.LoadScene("Loading");
    }

    public void ChangeGlobalVolume(float f){
        AudioListener.volume = f;
    }

    private void Awake()
    {
        if(PlayerPrefs.GetString("isAdmin") != "true"){
            adminWindow.SetActive(false);
        }

        sceneNames = new Dictionary<string, string>();
        animationRequirements = new Dictionary<int, float[]>();

        Physics.IgnoreLayerCollision(9, 10);
        interactionMap = new Map<int, string>();
        interactionMap.Add(1, "ShakeHand");
        interactionMap.Add(2, "ShakeHand0");
        interactionMap.Add(3, "ShakeHand1");
        interactionMap.Add(4, "Diploma");
        interactionMap.Add(5, "Diploma0");
        interactionMap.Add(6, "Diploma1");

        //interactionMap.Add(2, "TriggerShakeHand");
        if (!animationRequirements.ContainsKey(1))
            animationRequirements.Add(1, new[]{1.0f, 0.55f});
        if (!animationRequirements.ContainsKey(2))
            animationRequirements.Add(2, new[]{0.6f, 0.45f});

        animations = new[]
        {
            "OnGround",
            "Sit",
            "Crouch",
            "Clap",
            "Wave", 
            "Samba",
            "HipHop",
            "Cheer",
            //"IndividualShakeHand",
        };

        animationEnums = new[]
        {
            KeyCode.C,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            //KeyCode.H


        };

        if (!sceneNames.ContainsKey("1"))
            sceneNames.Add("1", "The Lobby");
        if (!sceneNames.ContainsKey("2"))
            sceneNames.Add("2", "The Circle");
    }

}





public class Map<T1, T2>
{
    private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
    private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

    public Map()
    {
        this.Forward = new Indexer<T1, T2>(_forward);
        this.Reverse = new Indexer<T2, T1>(_reverse);
    }

    public class Indexer<T3, T4>
    {
        private Dictionary<T3, T4> _dictionary;
        public Indexer(Dictionary<T3, T4> dictionary)
        {
            _dictionary = dictionary;
        }
        public T4 this[T3 index]
        {
            get { return _dictionary[index]; }
            set { _dictionary[index] = value; }
        }
    }

    public void Add(T1 t1, T2 t2)
    {
        _forward.Add(t1, t2);
        _reverse.Add(t2, t1);
    }

    public Indexer<T1, T2> Forward { get; private set; }
    public Indexer<T2, T1> Reverse { get; private set; }
}
