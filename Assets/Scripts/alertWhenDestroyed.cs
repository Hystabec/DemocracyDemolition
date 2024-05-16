using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alertWhenDestroyed : MonoBehaviour
{
    void OnDestroy()
    {
        Debug.LogError(gameObject.name + " destroyed - if tiggered let me (Eddie) what you did to cause it");
        Debug.Break();
    }
}
