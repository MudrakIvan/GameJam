using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{

    public AudioSource runSoundEffect;
    public AudioSource attackSoundEffect;
    public AudioSource jumpSoundEffect;
    public AudioSource dieSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunSound(){
        runSoundEffect.Play();
    }

    public void AttackSound(){
        attackSoundEffect.Play();
    }

    public void JumpSound(){
        jumpSoundEffect.Play();
    }

    public void DieSound(){
        dieSoundEffect.Play();
    }
}
