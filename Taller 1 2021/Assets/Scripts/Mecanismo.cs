using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mecanismo : MonoBehaviour
{
    public Interruptor[] interruptores;

    public UnityEvent customActions;

    // Update is called once per frame
    void Update()
    {
        bool activarEfecto = true;
        foreach (Interruptor i in interruptores)
        {
            if (!i.activado) activarEfecto = false;
        }

        if (activarEfecto)
        {
            //gameObject.SetActive(false);
            customActions.Invoke();
        }
    }
}
