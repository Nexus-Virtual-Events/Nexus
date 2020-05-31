using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMusic : MonoBehaviour
{
    AudioSource fxSound;
    public AudioClip backMusic;
    // Start is called before the first frame update
    void Start()
    {
        fxSound = GetComponent<AudioSource>();   
        Invoke("PlayMusic", 5.0f);
    }

    void PlayMusic(){
        fxSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
