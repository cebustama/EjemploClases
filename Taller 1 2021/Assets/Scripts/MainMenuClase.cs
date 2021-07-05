using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuClase : MonoBehaviour
{
    public int escenaInicioJuego = 1;

    public void ComenzarJuego()
    {
        SceneManager.LoadScene(escenaInicioJuego);
    }

    public void SalirDelJuego()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
