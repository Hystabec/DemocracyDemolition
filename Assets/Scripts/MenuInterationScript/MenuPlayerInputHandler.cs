using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPlayerInputHandler : MonoBehaviour
{
    PlayerInput pInput;
    int mPlayerIndex;
    MenuInputs PossibleMIOwner;
    MenuInputs MIOwner;

    bool hasTriedToAssign = false;

    float joinStickDeadZone = 0.5f;

    void Awake()
    {
        pInput = GetComponent<PlayerInput>();
        mPlayerIndex = pInput.playerIndex;

        var allMI = FindObjectsOfType<MenuInputs>();

        //this works as there will only be 1 "MenuInputs" in the menu - if you add more this wont work
        PossibleMIOwner = allMI[0];
    }

    void AnyInput()
    {
        if(!hasTriedToAssign)
        {
            if(!PossibleMIOwner.GetHasOwner())
            {
                PossibleMIOwner.SetHasOwner(true);
                PossibleMIOwner.SetPlayerIndex(mPlayerIndex);
                MIOwner = PossibleMIOwner;
            }

            hasTriedToAssign = true;
        }
    }

    void OnA()
    {
        AnyInput();

        if (MIOwner != null)
            MIOwner.A();
    }

    void OnB()
    {
        AnyInput();

        if (MIOwner != null)
            MIOwner.B();
    }

    void OnRB()
    {
        AnyInput();
    }

    void OnLB()
    {
        AnyInput();
    }

    void OnRT()
    {
        AnyInput();
    }

    void OnLS(InputValue value)
    {
        Vector2 val = value.Get<Vector2>();
        if (val.x > joinStickDeadZone || val.x < 0 - joinStickDeadZone && val.y > joinStickDeadZone || val.y < 0 - joinStickDeadZone)
        {
            AnyInput();

            if (MIOwner != null)
                MIOwner.LS(value);
        }
        else if(val.x > joinStickDeadZone || val.x < 0 - joinStickDeadZone)
        {
            AnyInput();

            if (MIOwner != null)
                MIOwner.FakeLS(new(val.x, 0));
        }
        else if(val.y > joinStickDeadZone || val.y < 0 - joinStickDeadZone)
        {
            AnyInput();

            if (MIOwner != null)
                MIOwner.FakeLS(new(0, val.y));
        }
        else
        {
            if (MIOwner != null)
                MIOwner.FakeLS(new(0, 0));
        }
    }

    void OnRS(InputValue value)
    {
        Vector2 val = value.Get<Vector2>();
        if (val.x > joinStickDeadZone || val.x < 0 - joinStickDeadZone || val.y > joinStickDeadZone || val.y < 0 - joinStickDeadZone)
        {
            AnyInput();
        }
    }

    void OnDUp(InputValue value)
    {
        AnyInput();

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
        AnyInput();

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
        AnyInput();

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
        AnyInput();

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
