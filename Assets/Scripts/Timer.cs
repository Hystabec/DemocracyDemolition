using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    float roundTime = 26f;

    float currentTime = 26f;

    bool timerRunning = false;
    public bool animTriggered = false;

    [SerializeField]
    TextMeshProUGUI timerText;

    [SerializeField]
    Animator timeText;

    public void timerReset()
    {
        currentTime = roundTime;
        UpdateTimerText();
    }

    void Awake()
    {
        currentTime = roundTime;
    }

    public void StartTime()
    {
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    void Update()
    {
        if (!timerRunning)
            return;

        if (currentTime >= 1f)
        {
            currentTime -= Time.deltaTime;
        }
        UpdateTimerText();

        if (currentTime <= 6f)
        {
            if (animTriggered == false)
            {
                timeText.SetTrigger("RunningOut");
                animTriggered = true;
            }
        }
    }

    void UpdateTimerText()
    {
        int roundSeconds = Mathf.FloorToInt(currentTime % 60f);

        string timeString = string.Format("{0}", roundSeconds);

        timerText.text = timeString;
    }
}
