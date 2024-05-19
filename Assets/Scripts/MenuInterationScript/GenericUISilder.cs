using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GenericUISilder : GenericMenuItem
{
    GenericUISilder()
    {
        Itemtype = MenuItemType.Slider;
    }

    [SerializeField]
    Slider slider;

    [SerializeField]
    GameObject[] HoveredItems;

    bool canSlide = false;

    public override void Activate()
    {
        canSlide = true;
    }

    public override void Deactivate()
    {
        canSlide = false;
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

    public override void GiveStickValues(Vector2 value)
    {
        if (!canSlide)
            return;

        if(value.x > 0)
        {
            slider.value += 0.1f;
        }
        else if(value.x < 0)
        {
            slider.value -= 0.1f;
        }
    }
}
