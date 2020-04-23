﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionRouter : MonoBehaviour
{
    private static GameObject localAvatar;
    private static GameObject currentChair;
    private static GameObject currentCharacter;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SetCurrentChair(GameObject o)
    {
        Debug.Log("current chair set");
        currentChair = o;
    }

    public static void SetLocalAvatar(GameObject o)
    {
        Debug.Log("[Action Router] Current Avatar set: " + o);
        localAvatar = o;
    }

    public static void SetCurrentCharacter(GameObject o)
    {
        Debug.Log("[Action Router] Current Character set");
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


