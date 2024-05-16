using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericUIButton : MonoBehaviour
{
    [SerializeField]
    UnityEvent thisEvent;

    [SerializeField]
    GameObject[] HoveredItems;

    public void ActivateButton()
    {
        HideHovered();
        thisEvent.Invoke();
    }

    public void ShowHovered()
    {
        foreach (GameObject go in HoveredItems)
            go.SetActive(true);
    }

    public void HideHovered()
    {
        foreach (GameObject go in HoveredItems)
            go.SetActive(false);
    }
}
