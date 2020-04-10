using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerName : MonoBehaviour
{

    public static string playerName;
    public TMP_Text displayName;

    // Start is called before the first frame update
    void Start()
    {
      displayName.text = playerName;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
