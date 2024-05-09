using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGameScript : MonoBehaviour
{
    [SerializeField]
    InGameManagerScript IGMS;

    [SerializeField]
    Animator camAnim, rematchAnim, menuButtonAnim;

    private void Awake()
    {
       
    }

    public void Retry()
    {
        Debug.Log("Rest game");
        //rematchAnim.SetTrigger("RematchFadeOut");
        //menuButtonAnim.SetTrigger("MenuButtonFadeOut");

        StartCoroutine(ButtonFadeOut());
        //IGMS.ResetGame();

        //zoom out camra - reverse zoom in anim?
    }

    private IEnumerator ButtonFadeOut()
    {
        //Delay for animation
        yield return new WaitForSeconds(1.2f);
        IGMS.CallResetGame();
    }

    public void Exit()
    {
        Debug.Log("Exit game");
        //go to main menu
    }
}
