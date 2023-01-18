using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider BGSlider;
    public Slider SFXSlider;

    public void SetBGVolume(float volume){
        audioMixer.SetFloat("BackgroundVolume", volume);
    }

    public void SetSFXVolume(float volume){
        audioMixer.SetFloat("SFXVolume", volume);
    }


    void Start()
    {
        float volume = 0;
        audioMixer.GetFloat("BackgroundVolume", out volume);
        BGSlider.value = volume;

        audioMixer.GetFloat("SFXVolume", out volume);
        SFXSlider.value = volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
