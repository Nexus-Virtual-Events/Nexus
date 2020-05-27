
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using Michsky.UI.ModernUIPack;
using TMPro;



public class Utils : MonoBehaviour
{
    public static Map<int, string> interactionMap;
    public static Dictionary<int, float[]> animationRequirements;
    public static Dictionary<string, string> sceneNames;
    public static string[] animations;
    public static UnityEngine.KeyCode[] animationEnums;

    public static List<GameObject> localPlayers;

    public static Dictionary<string, int> layerIndices;
    public static string[] layerNames;
    public GameObject adminWindow;
    public GameObject adminGlobalButton;
    public static string WebUrl = "http://www.thenexus.cloud/";

    public void MoveToRoom(string roomName)
    {
        Loading.sceneString = roomName;
        SceneManager.LoadScene("Loading");
    }

    public void ChangeGlobalVolume(float f){
        AudioListener.volume = f;
    }

    private int numberOfRooms = 9;

    private void Awake()
    {
        if(PlayerPrefs.GetString("isAdmin") != "true"){
            adminWindow.SetActive(false);
            adminGlobalButton.SetActive(false);
        }

        sceneNames = new Dictionary<string, string>();
        animationRequirements = new Dictionary<int, float[]>();
        layerIndices = new Dictionary<string, int>();
        localPlayers = new List<GameObject>();
        layerNames = new string[15];

        for(int i =0; i < numberOfRooms; i++){
            layerNames[i] = "Room" + (i+1).ToString();
        }
        layerNames[numberOfRooms + 1] = "Hidden";


        layerIndices.Add("Admin", 11);
        layerIndices.Add("Room 1", 12);
        layerIndices.Add("Room 2", 13);
        layerIndices.Add("Room 3", 14);
        layerIndices.Add("Room 4", 15);
        layerIndices.Add("Room 5", 16);
        layerIndices.Add("Room 6", 17);
        layerIndices.Add("Room 7", 18);
        layerIndices.Add("Room 8", 19);
        layerIndices.Add("Room 9", 20);
        layerIndices.Add("Hidden", 21);


        



        Physics.IgnoreLayerCollision(9, 10);
        interactionMap = new Map<int, string>();
        interactionMap.Add(1, "ShakeHand");
        interactionMap.Add(2, "ShakeHand0");
        interactionMap.Add(3, "ShakeHand1");
        interactionMap.Add(4, "Diploma");
        interactionMap.Add(5, "Diploma0");
        interactionMap.Add(6, "Diploma1");
        // interactionMap.Add(7, "Kick");


        //interactionMap.Add(2, "TriggerShakeHand");
        if (!animationRequirements.ContainsKey(1))
            animationRequirements.Add(1, new[]{1.0f, 0.55f});
        if (!animationRequirements.ContainsKey(2))
            animationRequirements.Add(4, new[]{0.6f, 0.45f});

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

        for(int i=11; i<22; i++){
            for(int j=11; j<22; j++){
                if(i == j)
                    continue;
                
                Physics.IgnoreLayerCollision(i, j);
            }
        }
    }

    // private float offsetx = 0f;
    // private float offsety = 1.25f;
    // private float offsetz = -2f;
    public static void AssignCameraToPlayer(GameObject player){
            Debug.Log(">>>" + LayerMask.LayerToName(player.layer));
            GameObject m_MainCamera = Camera.main.gameObject;
            Camera.main.gameObject.layer = player.layer;
            m_MainCamera.transform.parent = player.transform;
            Vector3 offset = new Vector3(0f, 1.25f, -2f);
            m_MainCamera.transform.position = player.transform.position + offset;
            m_MainCamera.transform.LookAt(player.transform);

            // playerCamera = m_MainCamera.transform;

            ThirdPersonOrbitCamBasic camScript = m_MainCamera.transform.GetComponent<ThirdPersonOrbitCamBasic>();
            camScript.AssignPlayer(player.transform);
        }

    public void AskMic(){
        StartCoroutine(AskForMicAccess());
    }    

    public static GameObject GetChildWithName(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    // public void KickPlayer(string s){
        
    // }

    public IEnumerator AskForMicAccess()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("Microphone found");
        }
        else
        {
            Debug.Log("Microphone not found");
        }
    
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
