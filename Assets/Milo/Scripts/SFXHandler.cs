using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip ough, punch, DeathSound;
    public void PlaySound()
    {
        audioSource.Play();
    }
}