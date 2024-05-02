using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Voume : MonoBehaviour
{
    public AudioMixer mixer;
    public void SetLevel(float sliderValue)
    {
        Debug.Log("volume");
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
}
