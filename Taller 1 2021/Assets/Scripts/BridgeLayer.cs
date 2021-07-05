using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class BridgeLayer : MonoBehaviour
{
    public TilemapCollider2D desactivarColision;
    public TilemapCollider2D activarColision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            desactivarColision.enabled = false;
            activarColision.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            desactivarColision.enabled = true;
            activarColision.enabled = false;
        }
    }
}
