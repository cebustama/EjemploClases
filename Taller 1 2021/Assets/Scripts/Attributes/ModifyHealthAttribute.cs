using UnityEngine;
using System.Collections;

[AddComponentMenu("Playground/Attributes/Modify Health")]
public class ModifyHealthAttribute : MonoBehaviour
{
	public bool destroyWhenActivated = false;
	public int healthChange = -1;

	public float force;
	public float shake;

	public string[] tagsExcepciones;
	public float tiempoInvencible;

	float invincibleTimer;

	//This will create a dialog window asking for which dialog to add
	private void Reset()
	{
		Utils.Collider2DDialogWindow(this.gameObject, true);
	}

	// This function gets called everytime this object collides with another
	private void OnCollisionEnter2D(Collision2D collisionData)
	{
		OnTriggerEnter2D(collisionData.collider);
	}

	private void OnTriggerEnter2D(Collider2D colliderData)
	{
		Rigidbody2D rb = colliderData.gameObject.GetComponent<Rigidbody2D>();
		if (rb != null)
        {
			Vector2 forceDirection = (colliderData.transform.position - transform.position).normalized;
			rb.AddForce(forceDirection * force, ForceMode2D.Impulse);
        }

		// Camera shake
		if (shake > 0)
        {
			//ScreenShakeManager.instance.AddShake(shake, 0.5f);
        }

		HealthSystemAttribute healthScript = colliderData.gameObject.GetComponent<HealthSystemAttribute>();
		if(healthScript != null)
		{
			// Si el otro objeto tiene cualquiera de las etiquetas de excepcion, cancelo el daño
			foreach (string tag in tagsExcepciones)
            {
				if (colliderData.gameObject.tag == tag)
					return;
            }

			// subtract health from the player
			healthScript.ModifyHealth(healthChange);

			if(destroyWhenActivated)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
