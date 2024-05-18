using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alertWhenDestroyed : MonoBehaviour
{
    void OnDestroy()
    {
        Debug.LogError(gameObject.name + " destroyed");
        Debug.Break();
    }
}
