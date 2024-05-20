using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Sceneloader : MonoBehaviour
{
    [SerializeField]
    Animator fadeAnim;

    [SerializeField]
    ParticleSystem redConfetti, blueConfetti, redStreamers, blueStreamers;
    //Not sure why I can't .Play() when its an array[]

    public void PlayGame()
    {
        redConfetti.Play();
        blueConfetti.Play();
        redStreamers.Play();
        blueStreamers.Play();
        StartCoroutine(AnimsBeforePlay());
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene("Harrison 2");
    }

    public IEnumerator AnimsBeforePlay()
    {
        yield return new WaitForSeconds(1.5f);
        fadeAnim.SetTrigger("FadeToBlack");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("protoDragDrop");
    }
}