using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using TMPro;

[Serializable]
public class InitialData
{
    public string[] rooms;
}
public class RoomData
{
    public string name;
    public string id;
    public int count;
}

public class UpdateRoomCounts : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] roomCells;

    private ArrayList data;
    public static Dictionary<string, int> roomCounts;

    private string url = Utils.WebUrl + "get_room_counts";
    void Start()
    {
        roomCells = GameObject.FindGameObjectsWithTag("RoomCell");
        roomCounts = new Dictionary<string, int>();
        // foreach(GameObject roomCell in roomCells){
        //     // Debug.Log(roomCell.name);
        // }
        InvokeRepeating("GetRoomCounts", 0f, 30f);
    }
    private void FindGameObjectsInChildWithTag(GameObject parent, string tag, List<GameObject> l)
        {
            Transform t = parent.transform;
    
            for (int i = 0; i < t.childCount; i++) 
            {
                if(t.GetChild(i).gameObject.tag == tag)
                {
                    l.Add(t.GetChild(i).gameObject);
                }
                    
            }
                
        }

    private void GetRoomCounts(){
        StartCoroutine(GetCounts());
    }
    
    private GameObject FindGameObjectInChildWithName(GameObject parent, string name)
        {
            // List<GameObject> children = new List<GameObject>();
            Transform t = parent.transform;
    
            for (int i = 0; i < t.childCount; i++) 
            {
                if(t.GetChild(i).name == name)
                {
                    return t.GetChild(i).gameObject;
                }
                    
            }
                
            return null;
        }

    IEnumerator GetCounts()
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
            parseJSON(www.text);
            foreach(GameObject roomCell in roomCells){
                List<GameObject> capacities = new List<GameObject>();
                GameObject normal = FindGameObjectInChildWithName(roomCell, "Normal");
                GameObject highlighted = FindGameObjectInChildWithName(roomCell, "Highlighted");
                FindGameObjectsInChildWithTag(normal, "Capacity", capacities);
                FindGameObjectsInChildWithTag(highlighted, "Capacity", capacities);
                
                // = FindGameObjectInChildWithTag(roomCell, "Capacity");
                Debug.Log(capacities.Count);
                foreach(GameObject capacity in capacities){
                    GameObject t = capacity.transform.Find("Text (TMP)").gameObject;
                    t.GetComponent<TMP_Text>().text = roomCounts[roomCell.name].ToString();
                }
            }
            // Debug.Log(JsonUtility.FromJson<InitialData>(www.text).rooms[0]);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }    
    }

    private string findBetween(string text, string s){
        int begin = text.IndexOf(s);
        int end = text.Substring(begin+1, text.Length-begin-1).IndexOf(s);
        return text.Substring(begin+1, end+begin-2);
    }

    private string findBetween(string text, string a, string b){
        int begin = text.IndexOf(a);
        int end = text.Substring(begin+1, text.Length-begin-1).IndexOf(b);
        return text.Substring(begin+2, end+begin-1);
    }

    private void parseJSON(string text){
        int begin = text.IndexOf("[");
        int end = text.IndexOf("]");
        text = text.Substring(begin, end-begin);

        do{
            begin = text.IndexOf("{");
            end = text.IndexOf("}");
            string raw = text.Substring(begin, end-begin+1);

            // raw.IndexOf("name");
            int nameIndex = raw.IndexOf("name");
            string nameToEnd = raw.Substring(nameIndex+5, raw.Length-nameIndex-5);
            string name = findBetween(nameToEnd, "\"");
            Debug.Log("name: " + name);

            int countIndex = raw.IndexOf("count");
            string countToEnd = raw.Substring(countIndex+6, raw.Length-countIndex-6);
            string count = findBetween(countToEnd,  "\"");
            Debug.Log("count: " + count);
            
            roomCounts.Add(name, Convert.ToInt32(count));
            
            // text.Substring(begin, end-begin+1);
            // Debug.Log(text.Substring(end-begin+3, text.Length-(end-begin+3)));
            if(end-begin+3 < text.Length){
                text = text.Substring(end-begin+3, text.Length-(end-begin+3));
            }
            else{
                text = "";
            }
        }
        while(end-begin+1 < text.Length);

    }

}
