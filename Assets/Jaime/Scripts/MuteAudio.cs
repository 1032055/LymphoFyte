using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteAudio : MonoBehaviour
{
    [SerializeField] private bool IsMuted;

    private void Start()
    {
        IsMuted = false;
    }

    public void MutePressed()
    {
        IsMuted = !IsMuted;

        AudioListener.pause = IsMuted;
    }

}
