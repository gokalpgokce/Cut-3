using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioSource menuAudioSource;
    private AudioClip _swipeClip;
    private AudioClip _clickClip;
    private AudioClip _dropClip;
    private AudioClip _explosionClip;
    private AudioClip _fireworksClip;
    
    private void Start()
    {
        _swipeClip = Resources.Load<AudioClip>("Swipe");
        _clickClip = Resources.Load<AudioClip>("Click");
        _dropClip = Resources.Load<AudioClip>("Drop");
        _explosionClip = Resources.Load<AudioClip>("SpecialExp");
        _fireworksClip = Resources.Load<AudioClip>("Fireworks");
        sfxAudioSource = GetComponent<AudioSource>();
    }

    public void PlayMenuSound()
    {
        menuAudioSource.Play();
    }
    
    public void StopMenuSound()
    {
        menuAudioSource.Stop();
    }

    public void PlaySwipeSound()
    {
        sfxAudioSource.PlayOneShot(_swipeClip);
    }

    public void PlayClickSound()
    {
        sfxAudioSource.PlayOneShot(_clickClip);
    }
    public void PlayDropSound()
    {
        sfxAudioSource.PlayOneShot(_dropClip);
    }
    public void PlayExplosionSound()
    {
        sfxAudioSource.PlayOneShot(_explosionClip);
    }
    public void PlayFireworksSound()
    {
        sfxAudioSource.clip = _fireworksClip;
        sfxAudioSource.Play();
    }

    public void StopFireworksSound()
    {
        sfxAudioSource.Stop();
        sfxAudioSource.clip = null;
    }
}
