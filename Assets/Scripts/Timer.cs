using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

struct clockEvent
{
    public clockEvent(float timeFromStart, UnityAction action)
    {
        this.timeFromStart = timeFromStart;
        this.action = action;
        this.markedForDeletion = false;
    }

    public float timeFromStart { get; }
    public UnityAction action { get; }

    private bool markedForDeletion;

    public void MarkForDeletion()
    {
        this.markedForDeletion = true;
    }

    public bool isMarkedForDeletion()
    {
        return this.markedForDeletion;
    }
}

public class Timer : MonoBehaviour
{
    [SerializeField]
    float roundTime = 26f;
    [SerializeField]
    float defautBuildTime = 10f, deafultFightTime = 15f;

    float currentTime = 26f;
    float timePassed = 0;

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
    AudioSource timerAudioSource;

    [SerializeField]
    AudioClip fiveSeconds, fightStart, buildStart;

    [SerializeField]
    Image timeProgressBar;

    List<clockEvent> events = new List<clockEvent>();
    List<int> indexToRemoveThisFrame = new List<int>();

    bool BetweenRounds = false;
    float BetweenRoundsTime = 2.0f;
    UnityAction funcToCallAfterTime = null;

    public void StartBetweenRounds(float time,  UnityAction funcToCallAfterTime)
    {
        this.funcToCallAfterTime = funcToCallAfterTime;
        BetweenRoundsTime = time;
        BetweenRounds = true;
        timerRunning = true;
    }

    public void AddedEvent(float timeToWaitFromRoundStart, UnityAction eventToCall)
    {
        events.Add(new clockEvent(timeToWaitFromRoundStart, eventToCall));
    }

    public void RemoveEvent(UnityAction actionToRemove)
    {
        for (int index = 0; index < events.Count; index++)
        {
            if (events[index].action == actionToRemove)
            {
                indexToRemoveThisFrame.Add(index);
                events[index].MarkForDeletion();
            }
        }
    }

    public void RemoveEvent(UnityAction actionToRemove, float timeToWaitFromRoundStart)
    {
        //removes only the first element that matches both the action and the time

        for(int index = 0; index < events.Count; index++)
        {
            if (events[index].action == actionToRemove && events[index].timeFromStart == timeToWaitFromRoundStart)
            {
                indexToRemoveThisFrame.Add(index);
                events[index].MarkForDeletion();
                return;
            }
        }
    }

    void asyncRemoveEvents()
    {
        //this should happen once all of the current frame events have been ran - so that if an event removes itself or something else then it wont throw a null ref

        indexToRemoveThisFrame.Reverse();

        foreach(int index in indexToRemoveThisFrame)
        {
            events.RemoveAt(index);
        }

        indexToRemoveThisFrame.Clear();
    }

    void runEvents()
    {
        //this is a very inefficient function - however it will only be use once or twice so it should be fine
        foreach (clockEvent CE in events)
        {
            if (timePassed > CE.timeFromStart && !CE.isMarkedForDeletion())
            {
                CE.action.Invoke();
            }
        }

        asyncRemoveEvents();
    }

    public void timerReset()
    {
        timePassed = 0.0f;
        currentTime = roundTime;
        buildTime = defautBuildTime;
        fightTime = deafultFightTime;
        fighting = false;
        UpdateTimerText();
    }

    void Awake()
    {
        if (fightStart == null || buildStart == null || fiveSeconds == null)
            timerAudioSource = null;

        currentTime = roundTime;
        buildTime = defautBuildTime;
        fightTime = deafultFightTime;
    }

    public void StartTime()
    {
        timerAudioSource.Stop();
        timerAudioSource?.PlayOneShot(buildStart, 0.3f);
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ResumeTimer()
    {
        timerRunning = true;
    }

    void Update()
    {
        if (!timerRunning)
            return;

        if(BetweenRounds)
        {
            if(BetweenRoundsTime > 0.0f)
                BetweenRoundsTime -= Time.deltaTime;
            else
            {
                funcToCallAfterTime?.Invoke();
                BetweenRounds = false;
                BetweenRoundsTime = 2.0f;
                funcToCallAfterTime = null;
            }

            return;
        }

        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            timePassed += Time.deltaTime;
        }

        if (!fighting)
        { 
            if (buildTime > 0f)
            {
                buildTime -= Time.deltaTime;
            }
        }

        else if(fighting)
        {
            if (fightTime > 0f)
            {
                fightTime -= Time.deltaTime;
            }

        }

        runEvents();
        UpdateTimerText();

        if (currentTime <= 6f)
        {
            if (animTriggered == false)
            {
                timeText.SetTrigger("RunningOut");
                animTriggered = true;
                timerAudioSource.Play();
            }
        }

        if(!(currentTime > 0))
        {
            timerRunning = false;
            timerFinished();
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
        timerAudioSource?.PlayOneShot(fightStart, 0.3f);
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
        //Stops timer text going to -1
        if (roundSeconds < 0)
        {
            roundSeconds = 0;
        }
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
