using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioClip _swipeClip;
    private AudioClip _clickClip;
    private AudioClip _dropClip;
    private AudioClip _explosionClip;
    
    private void Start()
    {
        _swipeClip = Resources.Load<AudioClip>("Swipe");
        _clickClip = Resources.Load<AudioClip>("Click");
        _dropClip = Resources.Load<AudioClip>("Drop");
        _explosionClip = Resources.Load<AudioClip>("SpecialExp");
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
    public void PlayDropSound()
    {
        audioSource.PlayOneShot(_dropClip);
    }
    public void PlayExplosionSound()
    {
        audioSource.PlayOneShot(_explosionClip);
    }
}
