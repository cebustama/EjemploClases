using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : Enemy
{
    public Transform target;
    public GameObject projectile;

    public float fireRadius;
    public float fireDelay;
    private float fireDelayTimer;

    private void FixedUpdate()
    {
        CheckDistance();
    }

    public void CheckDistance()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        // Don't move if staggered or attacking
        if (currentState == EnemyState.idle || currentState == EnemyState.walk)
        {
            fireDelayTimer -= Time.fixedDeltaTime;
            if (distance < fireRadius)
            {
                if (fireDelayTimer <= 0)
                {
                    Vector3 diff = target.transform.position - transform.position;
                    GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                    newProjectile.GetComponent<Projectile>().Launch(diff.normalized, gameObject.tag, gameObject.layer);

                    fireDelayTimer = fireDelay;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, fireRadius);
    }
}
