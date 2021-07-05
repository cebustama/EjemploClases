using UnityEngine;

public class MovimientoNoDiagonal : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Se está moviendo
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            float vX = Mathf.Abs(rb.velocity.x);
            float vY = Mathf.Abs(rb.velocity.y);

            if (vX > vY)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
}
