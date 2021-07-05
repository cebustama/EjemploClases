using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PauseManagerClase : MonoBehaviour
{
    private bool isPaused = false;

    public GameObject panelPause;
    public int escenaMenu = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            panelPause.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            panelPause.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Continuar()
    {
        TogglePause();
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenu);
    }
}
