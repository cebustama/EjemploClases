using UnityEngine;
using System.Collections;

[AddComponentMenu("Playground/Movement/Auto Move")]
[RequireComponent(typeof(Rigidbody2D))]
public class AutoMove : Physics2DObject
{
	// These are the forces that will push the object every frame
	// don't forget they can be negative too!
	public Vector2 direction = new Vector2(1f, 0f);

	public bool apuntarAlJugador = false;
	public bool velocidadConstante = false;
	public float velocidad = 1f;

	//is the push relative or absolute to the world?
	public bool relativeToRotation = true;

	Animator animator;

    private void Start()
    {
        if (apuntarAlJugador)
        {
			Transform player = GameObject.FindGameObjectWithTag("Player").transform;
			Vector3 dir = (player.position - transform.position).normalized;

			direction = dir;
        }

		if (velocidadConstante)
        {
			rigidbody2D.velocity = direction * velocidad;
        }

		animator = GetComponent<Animator>();
    }

    // FixedUpdate is called once per frame
    void FixedUpdate ()
	{
		if (!velocidadConstante)
        {
			Vector2 movement = direction * 2f;
			ManageAnimation(movement);

			if (relativeToRotation)
			{
				rigidbody2D.AddRelativeForce(movement);
			}
			else
			{
				rigidbody2D.AddForce(movement);
			}
		}
	}


	//Draw an arrow to show the direction in which the object will move
	void OnDrawGizmosSelected()
	{
		if(this.enabled)
		{
			float extraAngle = (relativeToRotation) ? transform.rotation.eulerAngles.z : 0f;
			Utils.DrawMoveArrowGizmo(transform.position, direction, extraAngle);
		}
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
