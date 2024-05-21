using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    //this is a simple audio manager script

    AudioSource m_AudioSource;

    [SerializeField]
    float defaultVolumeScale = 1.0f;

    [SerializeField]
    AudioClip defaultClip = null;

    [SerializeField]
    bool playOnAwake = false;

    float m_volume = 1.0f;

    void Awake()
    {
       //looks for existing audioSource
       TryGetComponent<AudioSource>(out m_AudioSource);

        //if one isnt found add a new one
        if(m_AudioSource == null)
            m_AudioSource = gameObject.AddComponent<AudioSource>();

        //sets the volume to be the default value
        m_AudioSource.volume = defaultVolumeScale;

        //sets the script volume based on the current volume of the audio source
        m_volume = m_AudioSource.volume;

        //if there is a default clip assign it
        if(defaultClip)
            m_AudioSource.clip = defaultClip;

        //sets the play on awake
        m_AudioSource.playOnAwake = playOnAwake;
        if(playOnAwake)
            m_AudioSource.Play();
    }

    public void changeVolume(float newVolume)
    {
        m_AudioSource.volume = newVolume;
        m_volume = newVolume;
    }

    public void PlayOnce(AudioClip clipToPlay, float volumeToPlayAt)
    {
        //overload plays clip once at passed in volume scale

        if(clipToPlay == null)
        {
            Debug.LogError("Null clip was passed to SoundManager in PlayOnce");
            return;
        }

        float tempVol = m_volume;

        if(volumeToPlayAt >= 0)
            tempVol = volumeToPlayAt;

        m_AudioSource.PlayOneShot(clipToPlay, tempVol);
    }

    public void PlayOnce(AudioClip clipToPlay)
    {
        //plays the passed in clip once

        PlayOnce(clipToPlay, -1.0f);
    }

    public void ChangeClip(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("Null clip was passed to SoundManager in ChangeClip");
            return;
        }

        //changes the current clip in the audio source

        m_AudioSource.clip = clip;
    }

    public AudioClip GetClip()
    {
        //gets the current clip in the audio source

        return m_AudioSource.clip;
    }    

    public void PlayClip()
    {
        //plays the current audio clip

        m_AudioSource?.Play();
    }
}
