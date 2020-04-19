using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRouter : MonoBehaviour
{
    private static GameObject localAvatar;
    private static GameObject currentChair;

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
        Debug.Log("current avatar set");
        localAvatar = o;
    }

    public static GameObject GetLocalAvatar()
    {
        return localAvatar;
    }

    public static GameObject GetCurrentChair()
    {
        return currentChair;
    }

    // ActionRouter.SetLocalAvatar(....);

}
