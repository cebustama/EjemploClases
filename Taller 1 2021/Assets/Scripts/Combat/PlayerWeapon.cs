using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    protected PlayerController player;
    protected bool equipped = false;
    protected bool thrown = false;

    protected Hit throwingHit;
    protected Rigidbody2D rb;

    [SerializeField]
    protected Collider2D throwCollider;

    private void Start()
    {
        throwingHit = GetComponent<Hit>();
        throwingHit.activated = false;

        rb = GetComponent<Rigidbody2D>();

        throwCollider.enabled = false;
    }

    public virtual void Update()
    {
        if (thrown)
        {
            rb.velocity *= 0.994f;
            if (rb.velocity.magnitude <= 0.1f)
            {
                rb.velocity = Vector2.zero;
                thrown = false;
                throwingHit.activated = false;
                throwCollider.enabled = false;
            }
        }
    }

    public virtual void Equip()
    {
        equipped = true;
    }

    public virtual void Unequip()
    {
        equipped = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (player == null) player = other.GetComponent<PlayerController>();

            // Pick up
            player.PickUpWeapon(transform);
        }
    }

    public void Throw(Vector2 force)
    {
        if (!thrown)
        {
            rb.velocity = force;

            throwingHit.activated = true;

            thrown = true;

            Invoke("EnableCollider", 0.2f);
        }
    }

    public void EnableCollider()
    {
        throwCollider.enabled = true;
    }
}
