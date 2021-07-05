using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coliseo : MonoBehaviour
{
    public List<GameObject> enemigos;

    public UnityEvent consecuencias;

    public bool todosMuertos = true;

    bool yaPaso = false;

    private void Update()
    {
        todosMuertos = true;
        for (int i = 0; i < enemigos.Count; i++)
        {
            if (enemigos[i] != null) todosMuertos = false;
        }

        if (todosMuertos && !yaPaso)
        {
            consecuencias.Invoke();
            yaPaso = true;
        }
    }
}
