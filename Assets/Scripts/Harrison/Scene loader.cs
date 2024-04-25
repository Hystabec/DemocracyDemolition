using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Sceneloader : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Proto Drag Drop");
    }
    public void QuitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}