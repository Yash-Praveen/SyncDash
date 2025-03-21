using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    GameObject menu,gameOver;

    private void Start()
    {
        if (PlayerPrefs.HasKey("restart"))
        {
            StartButton();
            PlayerPrefs.DeleteKey("restart");
        }
    }

    public void StartButton()
    {
        menu.SetActive(false);
        LevelManager.instance.play = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("restart", 1);
        SceneManager.LoadScene(0);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
