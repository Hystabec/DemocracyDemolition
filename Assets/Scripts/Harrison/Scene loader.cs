using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Sceneloader : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("play");
        SceneManager.LoadScene("protoDragDrop");
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
}