using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interruptor : MonoBehaviour
{
    public bool activado = false;

    public void SetActivado(bool estado)
    {
        activado = estado;
    }
}
