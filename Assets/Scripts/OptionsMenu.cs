using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetBGVolume(float volume){
        audioMixer.SetFloat("BackgroundVolume", volume);
    }

    public void SetSFXVolume(float volume){
        audioMixer.SetFloat("SFXVolume", volume);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
