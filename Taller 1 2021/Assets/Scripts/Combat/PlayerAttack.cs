using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode keyToAttack = KeyCode.Space;

    public GameObject[] weapons;

    public int currentWeapon = 0;

    public float tiempoAtaque;
    public GameObject effect;

    float timerAtaque;

    [System.Serializable]
    public enum AimType
    {
        MOVE_DIRECTION,
        MOUSE_DIRECTION
    }
    public AimType aimType;

    // Update is called once per frame
    void Update()
    {
        // Rotación del arma
        // Apuntar a última dirección de movimiento
        if (aimType == AimType.MOVE_DIRECTION)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                weapons[currentWeapon].transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                weapons[currentWeapon].transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                weapons[currentWeapon].transform.localEulerAngles = new Vector3(0, 0, 90);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                weapons[currentWeapon].transform.localEulerAngles = new Vector3(0, 0, 270);
            }
        }
        // Apuntar en dirección al mouse
        else if (aimType == AimType.MOUSE_DIRECTION)
        {
            Vector2 mousePos = Input.mousePosition;

            Vector2 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 diff = mousePos - objectPos;

            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            weapons[currentWeapon].transform.localEulerAngles = new Vector3(0, 0, angle);
        }

        // Activacion del arma
        if (Input.GetKeyDown(keyToAttack))
        {
            weapons[currentWeapon].SetActive(true);
            timerAtaque = tiempoAtaque;

            if (effect != null)
            {
                GameObject fx = Instantiate(effect, weapons[currentWeapon].transform.GetChild(0).position, Quaternion.identity);
                Destroy(fx, 2f);
            }
        }

        // Timer del arma
        timerAtaque = timerAtaque - Time.deltaTime;
        if (timerAtaque <= 0)
        {
            weapons[currentWeapon].SetActive(false);
        }
    }
}
