using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXhandler : MonoBehaviour
{
    public AudioSource audioSource; 

    public AudioClip ough, punch, DeathSound;
    public void PlaySound()
    {

        if (audioSource)
        {
            audioSource.Play();
            
        }
    }
}
