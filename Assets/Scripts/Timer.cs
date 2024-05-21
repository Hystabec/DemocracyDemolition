using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    float roundTime = 26f;
    [SerializeField]
    float defautBuildTime = 10f, deafultFightTime = 15f;

    float currentTime = 26f;

    float buildTime;
    float fightTime;

    bool timerRunning = false;
    public bool animTriggered = false;

    [SerializeField]
    TextMeshProUGUI timerText;

    [SerializeField]
    Animator timeText;

    bool fighting = false;


    UnityEvent timerEndedEvent = new();

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip roundEnd, fiveSeconds, roundStart;

    [SerializeField]
    Image timeProgressBar;


    public void timerReset()
    {
        currentTime = roundTime;
        buildTime = defautBuildTime;
        fightTime = deafultFightTime;
        fighting = false;
        UpdateTimerText();
    }

    void Awake()
    {
        if (roundStart == null || roundEnd == null || fiveSeconds == null)
            audioSource = null;

        currentTime = roundTime;
        buildTime = defautBuildTime;
        fightTime = deafultFightTime;
        audioSource?.PlayOneShot(roundStart);
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

        if (currentTime >= 0f)
        {
            currentTime -= Time.deltaTime;
            audioSource?.PlayOneShot(roundEnd);
        }

        if (!fighting)
        { 
            if (buildTime >= 0f)
            {
                buildTime -= Time.deltaTime;
            }
        }

        else if(fighting)
        {
            if (fightTime >= 0f)
            {
                fightTime -= Time.deltaTime;
            }

        }


        UpdateTimerText();

        if (currentTime <= 6f)
        {
            if (animTriggered == false)
            {
                timeText.SetTrigger("RunningOut");
                animTriggered = true;
                audioSource?.PlayOneShot(fiveSeconds);
            }
        }

        if(currentTime <= 0)
        {
            timerFinished();
            timerRunning = false;
        }
    }

    private void FixedUpdate()
    {
        if (fighting)
        {
            UpdateProgressBarFight();
        }

        else if(!fighting)
        {
            UpdateProgressBarBuild();
        }
    }

    public void SwitchToFight()
    {
        fighting = true;
        timeProgressBar.fillAmount = 1;

    }

    void UpdateProgressBarBuild()
    { 
        timeProgressBar.fillAmount = (buildTime % 60f) / defautBuildTime; 
    }

    void UpdateProgressBarFight()
    {
        timeProgressBar.fillAmount = (fightTime % 60f) / deafultFightTime;
    }

    void UpdateTimerText()
    {
        int roundSeconds = Mathf.FloorToInt(currentTime % 60f);

        string timeString = string.Format("{0}", roundSeconds);

        timerText.text = timeString;
    }

    public void AddListener(UnityAction eventToAdd)
    {
        timerEndedEvent.AddListener(eventToAdd);
    }

    void timerFinished()
    {
        timerEndedEvent.Invoke();
    }
}
