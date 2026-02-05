using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuScript : MonoBehaviour
{
    void Start()
    {
        // if the game manager still exists, get rid of it
        if (GameManager.Gary)
        {
            Destroy(GameManager.Gary.gameObject); // destroy the instance
        }
    }

    public void btn_StartTheGame()
    {
        SceneManager.LoadScene("Level01");
    }

    public void btn_GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void btn_GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void btn_Quit()
    {
        // tell the app to quit
        //Application.Quit();
        SceneManager.LoadScene("Level04");
    }
}