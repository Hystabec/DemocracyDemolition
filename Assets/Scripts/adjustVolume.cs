using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjustVolume : MonoBehaviour
{
    [SerializeField]
    bool adjustOnStart = false;

    public void UpdateVolumeFromPref()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume"); 
    }

    private void Start()
    {
        if(adjustOnStart)
        {
            UpdateVolumeFromPref();
        }
    }
}
