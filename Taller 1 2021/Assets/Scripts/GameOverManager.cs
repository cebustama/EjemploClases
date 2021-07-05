using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject panelGameOver;
    PlayerController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Si el jugar muere, va a estar desactivado
        if (!player.gameObject.activeSelf)
        {
            panelGameOver.SetActive(true);
        }
    }

    public void Continuar()
    {
        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        // Cargar la escena del Main Menu
        SceneManager.LoadScene(0);
    }
}
