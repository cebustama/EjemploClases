using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public GameObject gunContainer;

    public GameObject gun;

    public int currentGun = 0;

    [System.Serializable]
    public enum AimType
    {
        DIRECTION,
        MOUSE
    }

    public AimType aimType;

    // Update is called once per frame
    void Update()
    {
        // Ruedita hacia arriba en el mouse
        if (Input.mouseScrollDelta.y > 0)
        {
            NextGun();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            PrevGun();
        }

        if (aimType == AimType.DIRECTION)
        {
            // Rotación del arma
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                gun.transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                gun.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                gun.transform.localEulerAngles = new Vector3(0, 0, 90);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                gun.transform.localEulerAngles = new Vector3(0, 0, 270);
            }
        }
        else
        {
            //Vector2 shootDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            Vector2 mousePos = Input.mousePosition;

            Vector2 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 diff = mousePos - objectPos;

            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            gun.transform.localEulerAngles = new Vector3(0, 0, angle);

        }
    }

    public void NextGun()
    {
        currentGun = currentGun + 1;
        if (currentGun >= gunContainer.transform.childCount) currentGun = 0;

        gun.SetActive(false);
        gun = gunContainer.transform.GetChild(currentGun).gameObject;
        gun.SetActive(true);
    }

    public void PrevGun()
    {
        currentGun = currentGun - 1;
        if (currentGun < 0) currentGun = gunContainer.transform.childCount - 1;

        gun.SetActive(false);
        gun = gunContainer.transform.GetChild(currentGun).gameObject;
        gun.SetActive(true);
    }
}
