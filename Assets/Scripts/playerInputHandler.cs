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
        var PScripts = FindObjectsOfType<playerScript>();
        mPlayerIndex = pInput.playerIndex;
        
        PScript = PScripts.FirstOrDefault(m => m.getPlayerIndex() == mPlayerIndex);
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
}
