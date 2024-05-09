using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endGameScript : MonoBehaviour
{
    [SerializeField]
    InGameManagerScript IGMS;

    [SerializeField]
    Animator camAnim, rematchAnim, menuButtonAnim;

    bool retryPressed = false;

    private void Awake()
    {
       
    }

    public void Retry()
    {

        if (retryPressed)
            return;

        retryPressed = true;

        Debug.Log("Rest game");

        //rematchAnim.SetTrigger("RematchFadeOut");
        //menuButtonAnim.SetTrigger("MenuButtonFadeOut");

        //IGMS.CallResetGame();

        StartCoroutine(ButtonFadeOut());

        //IGMS.ResetGame();
        //zoom out camra - reverse zoom in anim?
    }

    private IEnumerator ButtonFadeOut()
    {
        //Delay for animation
        //menuButtonAnim.SetTrigger("MenuButtonFadeOut");
        IGMS.CallResetGame();
        yield return new WaitForSeconds(5.0f);
        retryPressed = false;
    }

    public void Exit()
    {
        if (retryPressed)
            return;

        Debug.Log("Exit game");
        //go to main menu
        SceneManager.LoadScene(SceneManager.GetSceneByName("Harrison 2").buildIndex);
    }
}
