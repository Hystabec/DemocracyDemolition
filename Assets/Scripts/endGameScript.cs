using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGameScript : MonoBehaviour
{
    [SerializeField]
    InGameManagerScript IGMS;

    [SerializeField]
    Animator camAnim;

    private void Awake()
    {
       
    }

    public void Retry()
    {
        Debug.Log("Rest game");

        IGMS.ResetGame();

        //zoom out camra - reverse zoom in anim?
    }

    public void Exit()
    {
        Debug.Log("Exit game");
        //go to main menu
    }
}
