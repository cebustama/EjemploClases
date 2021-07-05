using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    //interact,
    stagger,
    idle
}

public enum MovementType
{
    FREE,
    TILED
}

public enum ControlType
{
    Arrows,
    WASD,
}

public class PlayerController : MonoBehaviour
{
    public PlayerState currentState;

    [Header("Move Settings")]
    public ControlType controlType = ControlType.WASD;
    public MovementType movementType;
    public float moveSpeed = 5f;
    public bool flipLeft = false;

    [Header("Tile Movement")]
    [SerializeField]
    private float moveTime = .2f;

    [SerializeField]
    private float tileSize = 1f;

    public LayerMask collisionLayer;
    private bool moving;
    private Vector3 startingPos;
    private Vector3 targetPos;

    [Header("Weapons")]
    public PlayerWeapon equippedWeapon;

    public enum FlipDirection
    {
        NONE,
        IZQ,
        DER
    }

    public FlipDirection flipXAnimation = FlipDirection.NONE;

    SpriteRenderer playerRenderer;
    Animator animator;
    public Collider2D playerCollider;

    Vector2 colliderCenterPosition;

    // Collision detection
    Vector3 pointA;
    Vector3 pointB;

    float overlapWidth = 0.4f;
    float overlapHeight = 1.3f;

    Vector2 movement;

    public Rigidbody2D rb;

    [Header("Health")]
    public float startingHealth = 10;
    float maxHealth;
    float currentHealth;

    [Header("Colliders Ataque")]
    public Collider2D colliderArriba;
    public Collider2D colliderAbajo;
    public Collider2D colliderDerecha;
    public Collider2D colliderIzquierda;

    private Vector2Int playerDirection = Vector2Int.down;

    private void Start()
    {
        currentState = PlayerState.idle;

        animator = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();

        playerCollider = GetComponent<Collider2D>();
        colliderCenterPosition = playerCollider.bounds.center;

        rb = GetComponent<Rigidbody2D>();

        // Health
        maxHealth = startingHealth;
        currentHealth = maxHealth;
    }

    void Update()
    {
        colliderCenterPosition = playerCollider.bounds.center;

        if (movementType == MovementType.TILED)
        {
            ManageTileMovement();
        }
        else if (movementType == MovementType.FREE)
        {
            ManageFreeMovement();
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.Space) && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCoroutine());
        }

        // Lanzar arma
        if (Input.GetMouseButtonDown(1) && equippedWeapon != null)
        {
            ThrowWeapon();
        }
    }

    IEnumerator AttackCoroutine()
    {
        // Llamar a la animación de ataque y setear el estado "atacando"
        animator.SetTrigger("attack");
        currentState = PlayerState.attack;

        // Obtener dirección en que estoy mirando y activar el collider correspondiente
        DesactivarColliders();

        if (playerDirection == Vector2Int.right) colliderDerecha.enabled = true;
        if (playerDirection == Vector2Int.left) colliderIzquierda.enabled = true;
        if (playerDirection == Vector2Int.up) colliderArriba.enabled = true;
        if (playerDirection == Vector2Int.down) colliderAbajo.enabled = true;

        // Esperar un frame
        yield return null;

        // Obtener tiempo de la animación de ataque
        float attackTime = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        // Esperar ese tiempo
        yield return new WaitForSeconds(attackTime);

        // Terminar el estado de atacar
        DesactivarColliders();
        currentState = PlayerState.idle;
    }

    private void DesactivarColliders()
    {
        colliderArriba.enabled = false;
        colliderAbajo.enabled = false;
        colliderDerecha.enabled = false;
        colliderIzquierda.enabled = false;
    }

    private void FixedUpdate()
    {
        if (movementType == MovementType.FREE && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            change = movement.normalized;
            rb.MovePosition((Vector2)transform.position + change * moveSpeed * Time.fixedDeltaTime);

            // El personaje se está moviendo
            if (movement != Vector2.zero)
            {
                animator.SetBool("moving", true);

                // Dar vuelta el sprite al caminar a la izquierda
                playerRenderer.flipX = false;
                if (flipLeft && movement.x < 0)
                {
                    playerRenderer.flipX = true;
                }

                animator.SetFloat("moveX", change.x);
                animator.SetFloat("moveY", change.y);
            }
            // El personaje NO se está moviendo
            else
            {
                animator.SetBool("moving", false);
            }
        }
    }

    Vector2 change;
    private void ManageFreeMovement()
    {
        change = Vector2.zero;

        // Moving with the arrow keys
        if (controlType == ControlType.Arrows)
        {
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Input.GetAxisRaw("Horizontal") == 0) movement.x = 0;
            if (Input.GetAxisRaw("Vertical") == 0) movement.y = 0;
        }
        else
        {
            movement = new Vector2(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));
            if (Input.GetAxisRaw("Horizontal2") == 0) movement.x = 0;
            if (Input.GetAxisRaw("Vertical2") == 0) movement.y = 0;
        }

        change = movement.normalized;

        // Eje X
        if (Mathf.Abs(change.x) > Mathf.Abs(change.y))
        {
            // Derecha
            if (change.x > 0)
            {
                playerDirection = Vector2Int.right;
            }
            // Izquierda
            else if (change.x < 0)
            {
                playerDirection = Vector2Int.left;
            }
        }
        // Eje Y
        else
        {
            // Arriba
            if (change.y > 0)
            {
                playerDirection = Vector2Int.up;
            }
            // Abajo
            else if (change.y < 0)
            {
                playerDirection = Vector2Int.down;
            }
        }
    }

    public void ManageTileMovement()
    {
        // UP
        if (Input.GetKey(KeyCode.W) && !moving)
        {
            pointA = new Vector3(colliderCenterPosition.x - tileSize * overlapWidth, colliderCenterPosition.y - tileSize * overlapWidth, 0);
            pointB = new Vector3(colliderCenterPosition.x + tileSize * overlapWidth, colliderCenterPosition.y + tileSize * overlapHeight, 0);

            if (!Physics2D.OverlapArea(pointA, pointB, collisionLayer))
            {
                StartCoroutine(MovePlayer(Vector3.up));
                movement = Vector3.up;
            }
        }

        // LEFT
        if (Input.GetKey(KeyCode.A) && !moving)
        {
            pointA = new Vector3(colliderCenterPosition.x - tileSize * overlapHeight, colliderCenterPosition.y - tileSize * overlapWidth, 0);
            pointB = new Vector3(colliderCenterPosition.x + tileSize * overlapWidth, colliderCenterPosition.y + tileSize * overlapWidth, 0);

            if (!Physics2D.OverlapArea(pointA, pointB, collisionLayer))
            {
                StartCoroutine(MovePlayer(Vector3.left));
                movement = Vector3.left;
            }
        }

        // RIGHT
        if (Input.GetKey(KeyCode.S) && !moving)
        {
            pointA = new Vector3(colliderCenterPosition.x - tileSize * overlapWidth, colliderCenterPosition.y - tileSize * overlapHeight, 0);
            pointB = new Vector3(colliderCenterPosition.x + tileSize * overlapWidth, colliderCenterPosition.y + tileSize * overlapWidth, 0);

            if (!Physics2D.OverlapArea(pointA, pointB, collisionLayer))
            {
                StartCoroutine(MovePlayer(Vector3.down));
                movement = Vector3.down;
            }
        }

        // DOWN
        if (Input.GetKey(KeyCode.D) && !moving)
        {
            pointA = new Vector3(colliderCenterPosition.x - tileSize * overlapWidth, colliderCenterPosition.y - tileSize * overlapWidth, 0);
            pointB = new Vector3(colliderCenterPosition.x + tileSize * overlapHeight, colliderCenterPosition.y + tileSize * overlapWidth, 0);

            if (!Physics2D.OverlapArea(pointA, pointB, collisionLayer))
            {
                StartCoroutine(MovePlayer(Vector3.right));
                movement = Vector3.right;
            }
        }
    }

    // Tile Movement
    private IEnumerator MovePlayer(Vector3 direction)
    {
        moving = true;

        float elapsedTime = 0;

        startingPos = transform.position;
        targetPos = startingPos + direction;

        while (elapsedTime < moveTime)
        {
            // TODO: Mover Rigidbody con MovePosition
            transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        moving = false;
        movement = Vector3.zero;
    }

    public void Hit(float knockTime, float damage)
    {
        if (currentState != PlayerState.stagger)
        {
            currentHealth -= damage;
            if (currentHealth > 0)
            {
                StartCoroutine(KnockCo(knockTime));
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator KnockCo(float knockTime)
    {
        if (currentState != PlayerState.stagger)
        {
            currentState = PlayerState.stagger;
            yield return new WaitForSeconds(knockTime);

            rb.velocity = Vector2.zero;
            currentState = PlayerState.idle;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointA, 0.1f);
        Gizmos.DrawWireSphere(pointB, 0.1f);
    }

    public void PickUpWeapon(Transform newWeapon)
    {
        // Drop current
        DropEquippedWeapon();

        newWeapon.SetParent(transform);
        newWeapon.localPosition = Vector3.zero;
        equippedWeapon = newWeapon.GetComponent<PlayerWeapon>();
        equippedWeapon.Equip();
    }

    public void DropEquippedWeapon()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.Unequip();
            equippedWeapon.transform.SetParent(null);
            equippedWeapon = null;
        }
    }

    public void ThrowWeapon()
    {
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float throwForce = 50f;

        equippedWeapon.Throw(dir * throwForce);
        DropEquippedWeapon();
    }
}
