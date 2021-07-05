using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float detonateTime = 3f;
    private float detonateTimer;
    public float detonateRadius = 3f;
    public GameObject detonateEffect;

    public float detonateForce = 5f;

    private void OnEnable()
    {
        detonateTimer = detonateTime;
    }

    // Update is called once per frame
    void Update()
    {
        detonateTimer -= Time.deltaTime;
        if (detonateTimer <= 0f)
        {
            Detonate();
        }
    }

    private void Detonate()
    {
        Collider2D[] allColliders = Physics2D.OverlapCircleAll(transform.position, detonateRadius);
        foreach (Collider2D c in allColliders)
        {
            // Solo hitboxes
            if (c.isTrigger)
            {
                // Solo si tiene Rigidbody
                Rigidbody2D rb = c.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 dir = (c.transform.position - transform.position).normalized;
                    rb.AddForce(dir * detonateForce, ForceMode2D.Impulse);
                }
            }
        }

        if (detonateEffect != null) Instantiate(detonateEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detonateRadius);
    }
}
