using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MenuItemType
{
    Button,
    Tickbox,
    Slider,
    UnAssigned
}

public class GenericMenuItem : MonoBehaviour
{
    protected MenuItemType Itemtype = MenuItemType.UnAssigned;

    public MenuItemType GetItemType() {  return Itemtype; }
    public virtual void Activate() { }
    public virtual void Deactivate() { }
    public virtual void ShowHovered() { }
    public virtual void HideHovered() { }
    public virtual void GiveStickValues(Vector2 value) { }
}
