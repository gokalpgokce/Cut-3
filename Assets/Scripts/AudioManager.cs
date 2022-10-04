using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioClip _swipeClip;
    
    private void Start()
    {
        _swipeClip = Resources.Load<AudioClip>("Swipe");
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySwipeSound()
    {
        audioSource.PlayOneShot(_swipeClip);
    }
}
