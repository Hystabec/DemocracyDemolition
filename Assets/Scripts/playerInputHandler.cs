using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputHandler : MonoBehaviour
{
    PlayerInput pInput;
    int mPlayerIndex;
    playerScript PScript;

    void Awake()
    {
        pInput = GetComponent<PlayerInput>();
        mPlayerIndex = pInput.playerIndex;

        var PScripts = FindObjectsOfType<playerScript>();
        PScript = PScripts.FirstOrDefault(m => m.getPlayerIndex() == mPlayerIndex);

        if (PScript == null)
            Destroy(this.gameObject);
        else
            PScript.SetAssignedControllerIndex(mPlayerIndex);
    }

    void OnA()
    {
        if (PScript != null) 
            PScript.A();
    }

    void OnB()
    {
        if (PScript != null)
            PScript.B();
    }

    void OnStartButton()
    {
        if (PScript != null)
            PScript.StartButton();
    }

    void OnRB()
    {
        if (PScript != null)
            PScript.RB();
    }

    void OnLB()
    {
        if (PScript != null)
            PScript.LB();
    }

    void OnRT()
    {
        if (PScript != null)
            PScript.RT();
    }

    void OnLS(InputValue value)
    {
        if (PScript != null)
            PScript.LS(value);
    }

    void OnRS(InputValue value)
    {
        if (PScript != null)
            PScript.RS(value);
    }

    void OnDUp(InputValue value)
    {
        if (PScript != null)
        {
            if(value.isPressed)
            {
                PScript.DPad(new Vector2(0, 1));
            }
            else 
            { 
                PScript.DPad(new Vector2(0, -1));
            }
        }
    }

    void OnDDown(InputValue value)
    {
        if (PScript != null)
        {
            if (value.isPressed)
            {
                PScript.DPad(new Vector2(0, -1));
            }
            else
            {
                PScript.DPad(new Vector2(0, 1));
            }
        }
    }

    void OnDLeft(InputValue value)
    {
        if (PScript != null)
        {
            if (value.isPressed)
            {
                PScript.DPad(new Vector2(-1, 0));
            }
            else
            {
                PScript.DPad(new Vector2(1, 0));
            }
        }
    }

    void OnDRight(InputValue value)
    {
        if (PScript != null)
        {
            if (value.isPressed)
            {
                PScript.DPad(new Vector2(1, 0));
            }
            else
            {
                PScript.DPad(new Vector2(-1, 0));
            }
        }
    }
}
