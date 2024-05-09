using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraAnimationStatus : MonoBehaviour
{
    bool isInAnimation = false;
    
    
    public bool isAnimating()
    {
        return isInAnimation;
    }

    // Start is called before the first frame update
    public void animationStart()
    {
        isInAnimation = true;
    }

    // Update is called once per frame
    public void animationFinish()
    {
        isInAnimation = false;
    }

    public void testFunction()
    {
        Debug.Log("Success");
    }
}
