using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeactivate : MonoBehaviour
{
    public float tiempoParaDesactivarse = 5f;

    private void OnEnable()
    {
        Invoke("Desactivar", tiempoParaDesactivarse);
    }

    public void Desactivar()
    {
        gameObject.SetActive(false);
    }
}
