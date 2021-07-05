using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public ShootingType shootingType;

    public GameObject projectile;

    public float rateOfFire = 0.5f;
    float fireTimer = 0;

    private Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        fireTimer -= Time.deltaTime;

        bool isShooting = false;
        switch (shootingType)
        {
            case ShootingType.click:
                isShooting = Input.GetMouseButtonDown(0);
                break;
            case ShootingType.hold:
                isShooting = Input.GetMouseButton(0);
                break;
            case ShootingType.release:
                isShooting = Input.GetMouseButtonUp(0);
                break;
        }

        if (isShooting && fireTimer <= 0)
        {
            fireTimer =  1 / rateOfFire;
            GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            newProjectile.GetComponent<Projectile>().Launch(direction, gameObject.tag, gameObject.layer);
        }
    }
}
