using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    private bool triggered;
    public GameObject WinCanvas;
    public AudioSource BGMusic;
    public AudioSource BGWinMusic;

    private void Start()
    {
        triggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered)
            return;

        WinCanvas.SetActive(true);
        BGMusic.Stop();
        BGWinMusic.Play();
        
        triggered = true;
    }
}
