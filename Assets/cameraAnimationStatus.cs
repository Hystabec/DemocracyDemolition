using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraAnimationStatus : MonoBehaviour
{
    bool isInAnimation = false;
    bool hasStarted = false;

    public void Reset()
    {
        isInAnimation = false;
        hasStarted = false;
    }

    public bool isAnimating()
    {
        return isInAnimation;
    }

    public bool Started()
    {
        return hasStarted;
    }

    public void animationStart()
    {
        Debug.Log("anim start");
        isInAnimation = true;
        hasStarted = true;
    }

    public void animationFinish()
    {
        Debug.Log("anim stop");
        isInAnimation = false;
    }
}

