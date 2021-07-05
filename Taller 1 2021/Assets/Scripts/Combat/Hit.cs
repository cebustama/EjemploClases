using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public float thrust = 4f;
    public float knockTime = 0.3f;
    public float damage;

    public List<string> tagHitFilterList = new List<string>();
    public List<int> layerHitFilterList = new List<int>();

    public bool activated = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated) return;

        // Ignore hit for tag exceptions
        foreach (string tag in tagHitFilterList)
        {
            if (other.CompareTag(tag))
            {
                return;
            }
        }

        // Ignore hit for layers
        foreach (int layer in layerHitFilterList)
        {
            if (other.gameObject.layer == layer)
            {
                return;
            }
        }

        // Find enemy rb and apply force to it
        Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
        if (otherRb == null) otherRb = other.GetComponentInParent<Rigidbody2D>();

        // TODO: Separate hitboxes and hurtboxes into child objects, for now check for trigger
        if (otherRb != null && other.isTrigger)
        {
            // Apply force to respective rigid body
            Vector2 dir = (otherRb.transform.position - transform.position).normalized;
            otherRb.AddForce(dir * thrust, ForceMode2D.Impulse);

            // Stagger enemy
            Enemy e = other.GetComponent<Enemy>();
            if (e == null) e = other.GetComponentInParent<Enemy>();

            if (e != null)
            {
                e.Hit(otherRb, knockTime, damage);
            }

            // Stagger player
            PlayerController player = other.GetComponent<PlayerController>();
            if (player == null) player = other.GetComponentInParent<PlayerController>();

            if (player != null)
            {
                player.Hit(knockTime, damage);
            }
        }
    }
}
