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
    string PlayerPrefBool;

    Color boxTrueCol = Color.green, boxFalseCol = Color.red;

    bool currentVal = true;

    [SerializeField]
    GameObject[] HoveredItems;

    void Awake()
    {
        var intRet = PlayerPrefs.GetInt(PlayerPrefBool);

        if (intRet == 1)
        {
            currentVal = true;
            colourBox.color = boxTrueCol;
            overrideActive(true);
        }
        else
        {
            currentVal = false; 
            colourBox.color = boxFalseCol;
            overrideActive(false);
        }
    }

    void overrideActive(bool val)
    {
        PlayerPrefs.SetInt(PlayerPrefBool, val ? 1 : 0);
    }

    public override void Activate()
    {
        PlayerPrefs.SetInt(PlayerPrefBool, !currentVal ? 1 : 0);
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
