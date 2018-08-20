using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            LoadGame();
        }
    }

    public void OnStartClick()
    {
        LoadGame();
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
}
