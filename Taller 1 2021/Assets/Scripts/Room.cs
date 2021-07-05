using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCamera;
    private CinemachineVirtualCamera vc;

    private void Awake()
    {
        vc = virtualCamera.GetComponent<CinemachineVirtualCamera>();
        vc.Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            Debug.Log("Player entró a habitación " + gameObject.name);
            // Logica cuando entra el player
            virtualCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            Debug.Log("Player salií de habitación " + gameObject.name);
            // Logica cuando sale el player
            virtualCamera.SetActive(false);
        }
    }
}
