using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KickPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Kick(){
        Debug.Log("Kicking yo");
        GameObject name = Utils.GetChildWithName(gameObject, "PlayerManagementCard");
        string name_string = name.GetComponent<TMP_Text>().text;
        Debug.Log(name_string);
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
                if(Utils.GetChildWithName(player, "Player Name").GetComponent<TMP_Text>().text == name_string){
                    Debug.Log("Found someone");
                    player.GetComponent<ThirdPersonUserControl>().KickPlayer();
                }
            }
    }
        

    // Update is called once per frame
    void Update()
    {
        
    }
}
