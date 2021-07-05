using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    follow,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{
    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Enemy Settings")]
    public int maxHealth;
    float health;

    public string enemyName;

    public Transform target;

    [Header("Follow")]
    public float followDistance = 0f;
    public float followSpeed = 1f;
    public bool stopShooting = true;

    [Header("Attack")]
    public float attackDistance = 0f;
    public float attackRate = 1f;
    private float attackTimer = 0f;

    [Header("Shooting")]
    public float shootingDistance = 0f;
    public float shootingRate = 1f;
    private float shootingTimer;
    public GameObject bulletPrefab;

    [Header("Effects")]
    public GameObject deathEffect;
    private float deathEffectDeathTime = 1f;

    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public Animator animator;

    public const float arrivedDistance = 0.5f;

    protected Vector3 homePosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        homePosition = transform.position;
        health = maxHealth;

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        transform.position = homePosition;
        health = maxHealth;
        currentState = EnemyState.idle;
    }

    private void Update()
    {
        Vector2 diff = (target.position - transform.position);
        Vector2 targetDirection = diff.normalized;
        float targetDistance = diff.magnitude;

        // Attack
        attackTimer -= Time.deltaTime;
        if (targetDistance <= attackDistance)
        {
            if (attackTimer <= 0)
            {
                StartCoroutine(AttackCo());
            }
        }
        else if (attackTimer > 0)
        {
            currentState = EnemyState.idle;
        }

        // Follow
        if (currentState != EnemyState.attack)
        {
            if (targetDistance <= followDistance)
            {
                // Seguir al target
                rb.MovePosition(transform.position + (Vector3)targetDirection * followSpeed * Time.deltaTime);

                currentState = EnemyState.follow;
            }
            else
            {
                currentState = EnemyState.idle;
            }
        }

        // Shooting
        shootingTimer -= Time.deltaTime;
        if (targetDistance <= shootingDistance)
        {
            // Evitar disparar si nos está persiguiendo o atacando
            if ((currentState == EnemyState.follow || currentState == EnemyState.attack)
                && stopShooting) 
                shootingTimer = shootingRate;

            if (shootingTimer <= 0)
            {
                Projectile p = Instantiate(bulletPrefab).GetComponent<Projectile>();

                // Position del proyectil
                p.transform.position = transform.position;

                // Rotacion del proyectil
                float angle = Vector2.SignedAngle(Vector2.down, targetDirection);
                p.transform.localEulerAngles = new Vector3(0, 0, angle);

                p.Launch(targetDirection, layerException: gameObject.layer);
                shootingTimer = shootingRate;
            }
        }
    }

    public IEnumerator AttackCo()
    {
        animator?.SetTrigger("Attack");
        currentState = EnemyState.attack;

        yield return new WaitForEndOfFrame();
        float attackAnimLenght = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(attackAnimLenght);

        attackTimer = attackRate;
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DeathEffect();
            this.gameObject.SetActive(false);
        }
    }

    private void DeathEffect()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDeathTime);
        }
    }

    public void Hit(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        //Debug.Log("Soy " + enemyName + " y me están atacando");
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }

    IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null && currentState != EnemyState.stagger)
        {
            currentState = EnemyState.stagger;
            yield return new WaitForSeconds(knockTime);

            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootingDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
