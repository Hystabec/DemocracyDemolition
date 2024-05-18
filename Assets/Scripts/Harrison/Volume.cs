using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    [SerializeField] TMPro.TextMeshProUGUI text;

    private void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }

        text.text = "Volume: " + (int)(100 * volumeSlider.value) + "%";
    }
    public void ChangeVolume()
    {
        Debug.Log("volume");
        AudioListener.volume = volumeSlider.value;
        Save();
        text.text = "Volume: " + (int)(100 * volumeSlider.value) + "%";
    }

    private void Load ()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
