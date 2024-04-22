using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float roundTime = 20f;

    [SerializeField]
    TextMeshProUGUI timerText;

    void Update()
    {
        if (roundTime > 0f)
        {
            roundTime -= Time.deltaTime;
        }
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        int roundSeconds = Mathf.FloorToInt(roundTime % 60f);

        string timeString = string.Format("{0}", roundSeconds);

        timerText.text = timeString;
    }
}
