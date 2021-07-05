using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("Playground/Attributes/Health System")]
public class HealthSystemAttribute : MonoBehaviour
{
	public int health = 3;

	public Color hitColor = Color.white;


	private UIScript ui;
	private int maxHealth;

	// Will be set to 0 or 1 depending on how the GameObject is tagged
	// it's -1 if the object is not a player
	private int playerNumber;

	public UnityEvent cuandoMuera;

	private void Start()
	{
		// Find the UI in the scene and store a reference for later use
		ui = GameObject.FindObjectOfType<UIScript>();

		// Set the player number based on the GameObject tag
		switch(gameObject.tag)
		{
			case "Player":
				playerNumber = 0;
				break;
			case "Player2":
				playerNumber = 1;
				break;
			default:
				playerNumber = -1;
				break;
		}

		// Notify the UI so it will show the right initial amount
		if(ui != null
			&& playerNumber != -1)
		{
			ui.SetHealth(health, playerNumber);
		}

		maxHealth = health; //note down the maximum health to avoid going over it when the player gets healed
	}


	// changes the energy from the player
	// also notifies the UI (if present)
	public void ModifyHealth(int amount)
	{
		// Si le están haciendo daño
		if (amount < 0)
        {
			// Revisar si tiene un Sprite Renderer
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			if (sr != null)
            {
				sr.color = hitColor;
				Invoke("VolverAColorBase", 0.5f);

			}
        }


		//avoid going over the maximum health by forcin
		if(health + amount > maxHealth)
		{
			amount = maxHealth - health;
		}
			
		health += amount;

		// Notify the UI so it will change the number in the corner
		if(ui != null
			&& playerNumber != -1)
		{
			ui.ChangeHealth(amount, playerNumber);
		}

		// DEAD
		if (health <= 0)
		{
			// PASA ALGO QUE YO QUIERO QUE PASE
			cuandoMuera.Invoke();

			Animator animator = GetComponent<Animator>();
			if (animator != null)
            {
				animator.SetTrigger("Morir");
				Destroy(gameObject, 1f);
            }
            else
            {
				Destroy(gameObject);
			}
		}
	}

	public void VolverAColorBase()
    {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (sr != null)
		{
			sr.color = Color.white;
		}
	}
}
