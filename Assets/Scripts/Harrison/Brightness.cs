using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    public Slider sliderRef;
    public Light lightRef;
    void OnEnable()
    {
        sliderRef.onValueChanged.AddListener(sliderCallBack);
    }
    void sliderCallBack(float value)
    {
        Debug.Log("Slider Value Changed: " + value);
        lightRef.intensity = sliderRef.value;
    }
    void OnDisable()
    {
        sliderRef.onValueChanged.RemoveAllListeners();
    }
}
