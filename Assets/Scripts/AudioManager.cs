using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioClip _swipeClip;
    private AudioClip _clickClip;
    
    private void Start()
    {
        _swipeClip = Resources.Load<AudioClip>("Swipe");
        _clickClip = Resources.Load<AudioClip>("Click");
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySwipeSound()
    {
        audioSource.PlayOneShot(_swipeClip);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(_clickClip);
    }
}
