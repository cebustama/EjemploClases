using UnityEngine;
using System.Collections;

[AddComponentMenu("Playground/Movement/Follow Target")]
[RequireComponent(typeof(Rigidbody2D))]
public class FollowTarget : Physics2DObject
{
	// This is the target the object is going to move towards
	public Transform target;

	public float minDistance = float.PositiveInfinity;
	public bool followForever = false;
	bool startedFollowing = false;

	[Header("Movement")]
	// Speed used to move towards the target
	public float speed = 1f;
	public bool constantSpeed = true;

	// Used to decide if the object will look at the target while pursuing it
	public bool lookAtTarget = false;

	// The direction that will face the target
	public Enums.Directions useSide = Enums.Directions.Up;

	bool stopped = true;

	Animator animator;

	[HideInInspector]
	public bool followingTarget = false;

    private void Start()
    {
		animator = GetComponent<Animator>();
    }

    // FixedUpdate is called once per frame
    void FixedUpdate ()
	{
		//do nothing if the target hasn't been assigned or it was detroyed for some reason
		if (target == null)
        {
			target = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

		//look towards the target
		if(lookAtTarget)
		{
			Utils.SetAxisTowards(useSide, transform, target.position - transform.position);
		}

		//Move towards the target
		Vector2 targetPos;
		Vector3 targetDir = (target.position - transform.position).normalized;
		if (speed > 0)
        {
			targetPos = target.position;
        }
        else
        {
			targetPos = transform.position - targetDir * 3f;
        }

		// Seguir solo si la distancia es inferior a este número
		if ((target.position - transform.position).magnitude < minDistance
			|| (followForever && startedFollowing))
        {
			// Una vez que lo empieza a seguir, no para
			if (followForever)
            {
				startedFollowing = true;
            }

			followingTarget = true;

			Vector2 movement = Vector2.zero;
			stopped = false;
			if (constantSpeed)
			{
				movement = (Vector2)targetDir * Mathf.Abs(speed);
				rigidbody2D.AddForce(movement);
			}
			else
			{
				movement = Vector2.Lerp(transform.position, targetPos, Time.fixedDeltaTime * Mathf.Abs(speed));
				rigidbody2D.MovePosition(movement);
			}
			ManageAnimation(movement);
		}
		else
        {
			// Dejar de seguir
			if (!stopped)
            {
				rigidbody2D.velocity = Vector2.zero;
				stopped = true;
				followingTarget = false;

				ManageAnimation(Vector2.zero);
            }
        }
	}

	public void SetSpeed(float newSpeed)
    {
		speed = newSpeed;
    }

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, minDistance);
    }

	public void ManageAnimation(Vector2 movement)
	{
		if (animator == null) return;

		if (movement != Vector2.zero)
		{
			animator.SetBool("moving", true);
			animator.SetFloat("moveX", movement.x);
			animator.SetFloat("moveY", movement.y);
		}
		else
		{
			animator.SetBool("moving", false);
		}
	}
}
