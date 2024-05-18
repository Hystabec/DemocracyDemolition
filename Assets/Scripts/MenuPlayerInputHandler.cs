using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPlayerInputHandler : MonoBehaviour
{
    PlayerInput pInput;
    int mPlayerIndex;
    MenuInputs MIOwner;

    void Awake()
    {
        pInput = GetComponent<PlayerInput>();
        mPlayerIndex = pInput.playerIndex;

        var allMI = FindObjectsOfType<MenuInputs>();
        MIOwner = allMI.FirstOrDefault(m => m.GetRequestedPlayerIndex() == mPlayerIndex);

        if (MIOwner == null)
            Destroy(this.gameObject);
        else
            MIOwner.SetPlayerIndex(mPlayerIndex);
    }

    void OnA()
    {
        if(MIOwner != null)
            MIOwner.A();
    }

    void OnB()
    {

    }

    void OnRB()
    {

    }

    void OnLB()
    {

    }

    void OnRT()
    {

    }

    void OnLS(InputValue value)
    {
        if (MIOwner != null)
            MIOwner.LS(value);
    }

    void OnRS(InputValue value)
    { 

    }

    void OnDUp(InputValue value)
    {
        if (MIOwner != null)
        {
            if (value.isPressed)
            {
                MIOwner.DPad(new Vector2(0, 1));
            }
            else
            {
                MIOwner.DPad(new Vector2(0, -1));
            }
        }
    }

    void OnDDown(InputValue value)
    {
        if (MIOwner != null)
        {
            if (value.isPressed)
            {
                MIOwner.DPad(new Vector2(0, -1));
            }
            else
            {
                MIOwner.DPad(new Vector2(0, 1));
            }
        }
    }

    void OnDLeft(InputValue value)
    {
        if (MIOwner != null)
        {
            if (value.isPressed)
            {
                MIOwner.DPad(new Vector2(-1, 0));
            }
            else
            {
                MIOwner.DPad(new Vector2(1, 0));
            }
        }
    }

    void OnDRight(InputValue value)
    {
        if (MIOwner != null)
        {
            if (value.isPressed)
            {
                MIOwner.DPad(new Vector2(1, 0));
            }
            else
            {
                MIOwner.DPad(new Vector2(-1, 0));
            }
        }
    }
}
