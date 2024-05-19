using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GenericUIButton : GenericMenuItem
{
    GenericUIButton()
    {
        Itemtype = MenuItemType.Button;
    }

    [SerializeField]
    UnityEvent thisEvent;

    [SerializeField]
    GameObject[] HoveredItems;

    public override void Activate()
    {
        HideHovered();
        thisEvent.Invoke();
    }

    public override void ShowHovered()
    {
        foreach (GameObject go in HoveredItems)
            go.SetActive(true);
    }

    public override void HideHovered()
    {
        foreach (GameObject go in HoveredItems)
            go.SetActive(false);
    }
}
