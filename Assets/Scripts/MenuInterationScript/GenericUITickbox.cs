using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericUITickbox : GenericMenuItem
{
    GenericUITickbox()
    {
        Itemtype = MenuItemType.Tickbox;
    }

    [SerializeField]
    UnityEngine.UI.Image colourBox;

    [SerializeField]
    UnityEvent<bool> OnValueChange;

    Color boxTrueCol = Color.green, boxFalseCol = Color.red;

    [SerializeField]
    bool currentVal = true;

    [SerializeField]
    GameObject[] HoveredItems;

    void Awake()
    {
        if (currentVal)
        {
            colourBox.color = boxTrueCol;
            overrideActive(true);
        }
        else
        {
            colourBox.color = boxFalseCol;
            overrideActive(false);
        }
    }

    void overrideActive(bool val)
    {
        OnValueChange.Invoke(val);
    }

    public override void Activate()
    {
        OnValueChange.Invoke(!currentVal);
        currentVal = !currentVal;

        if(currentVal) 
            colourBox.color = boxTrueCol;
        else
            colourBox.color = boxFalseCol;
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
