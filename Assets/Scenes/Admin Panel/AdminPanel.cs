using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdminPanel : MonoBehaviour
{
    public TMP_Text buttonText;
    public TMP_Text buttonHighlightedText;

    private int _eventStatus = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeEventStatus(int eventIndex)
    {
        if(buttonText.text == "START")
        {
            buttonText.text = "END";
            buttonHighlightedText.text = "CONFIRM";

            _eventStatus = 1;
        }
        else
        {
            buttonText.text = "END";
            buttonHighlightedText.text = "CONFIRM";

            _eventStatus = 0;
        }
        
    }
}
