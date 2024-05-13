using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericUIButton : MonoBehaviour
{
    [SerializeField]
    UnityEvent thisEvent;

    public void ActivateButton()
    {
        thisEvent.Invoke();
    }
}
