using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingType
{
    click,
    hold,
    release,
    charge
}

public class RangedWeapon : PlayerWeapon
{
    public ShootingType shootingType;
    public GameObject projectile;

    public List<Transform> firePoints;

    [Header("Settings")]
    public float fireRate = 0.5f;
    float fireTimer = 0;
    //[Range(0, 1)]
    //public float accuracy = 1f;
    public float maxAccAngle = 45f;
    public float recoilForce = 1f;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (equipped)
        {
            // Apuntar hacia el mouse
            Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            float mouseAngle = Vector2.SignedAngle(Vector2.right, dir);

            transform.localEulerAngles = new Vector3(0, 0, mouseAngle);

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
                Shoot(dir);
            }
        }
    }

    public virtual void Shoot(Vector2 direction)
    {
        fireTimer = 1 / fireRate;

        // Disparar por cada punto de disparo
        for (int i = 0; i < firePoints.Count; i++)
        {
            GameObject newProjectile = Instantiate(projectile, firePoints[i].transform.position, Quaternion.identity);
            Vector2 rotatedDirection = RotateVector(direction, firePoints[i].localEulerAngles.z);

            // Rotar el ángulo de disparo en el rango de accuracy definido
            float randomRotation = Random.Range(-maxAccAngle, maxAccAngle);
            rotatedDirection = RotateVector(rotatedDirection, randomRotation);

            newProjectile.GetComponent<Projectile>().Launch(rotatedDirection, gameObject.tag, gameObject.layer);
        }

        // Aplicar el recoil al player
        if (recoilForce != 0)
        {
            Vector2 recoilDirection = direction * -1f;
            player.rb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse);
            player.Hit(.5f, 0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            for (int i = 0; i < firePoints.Count; i++)
            {
                Gizmos.color = Color.white;
                Vector2 initial = firePoints[i].transform.position;
                Vector2 direction = RotateVector(Vector2.right, firePoints[i].localEulerAngles.z).normalized;
                Vector2 final = initial + direction * .5f;
                Gizmos.DrawLine(initial, final);

                // Accuracy
                Gizmos.color = Color.red;
                Vector2 topAcc = RotateVector(direction, maxAccAngle);
                Vector2 bottomAcc = RotateVector(direction, -maxAccAngle);
                Gizmos.DrawLine(initial, initial + topAcc * .25f);
                Gizmos.DrawLine(initial, initial + bottomAcc * .25f);
            }

            
        }
    }

    // https://answers.unity.com/questions/1229302/rotate-a-vector2-around-the-z-axis-on-a-mathematic.html
    Vector2 RotateVector(Vector2 aPoint, float aDegree)
    {
        float rad = aDegree * Mathf.Deg2Rad;
        float s = Mathf.Sin(rad);
        float c = Mathf.Cos(rad);
        return new Vector2(
            aPoint.x * c - aPoint.y * s,
            aPoint.y * c + aPoint.x * s);
    }

}
