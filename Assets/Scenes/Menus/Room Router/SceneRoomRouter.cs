using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRoomRouter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] userRooms;
    public static string currentLayer;

    public GameObject[] roomTabs;
    LayerMask myMask;
    void Start()
    {

        if(PlayerPrefs.GetString("adminRoom") == "true"){
            foreach(GameObject room in userRooms){
                room.SetActive(true);
            }

            myMask = Camera.main.cullingMask;
            foreach(string layerName in Utils.layerNames){
                myMask = makeLayerInvisible(myMask, LayerMask.NameToLayer(layerName));
            }
            currentLayer = Utils.layerNames[0];
            myMask = makeLayerVisible(myMask, LayerMask.NameToLayer(currentLayer));
            Camera.main.cullingMask = myMask;
        }    
        else{
            foreach(GameObject room in roomTabs){
                room.SetActive(false);
            }
        }    
        
      

    }

    private LayerMask makeLayerInvisible(LayerMask mask, int i){
        return mask & ~(1 << i);
        // Debug.Log(">>>");
    }
    private LayerMask makeLayerVisible(LayerMask mask, int i){
       return mask | (1 << i);
        // Debug.Log(">>>");
    }

    public void ChangeRoomView(string newLayer){
        myMask = Camera.main.cullingMask;
        myMask = makeLayerInvisible(myMask, LayerMask.NameToLayer(currentLayer));
        myMask = makeLayerVisible(myMask, LayerMask.NameToLayer(newLayer));
        currentLayer = newLayer;
        Camera.main.cullingMask = myMask;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
