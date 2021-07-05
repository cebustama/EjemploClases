using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Playground/Conditions/Condition Resource")]
public class ConditionResource : ConditionBase
{
	[Header("Resource")]

	public int checkFor = 0;
	public int amountNeeded = 1;

	private float timeLastEventFired;

	private UIScript userInterface;

	bool done = false;


	private void Start()
	{
		userInterface = GameObject.FindObjectOfType<UIScript>();
	}


	private void Update()
	{
		// Chequear que existe el recurso
		if (userInterface.CheckIfHasResources(checkFor, amountNeeded) && !done)
		{
			ExecuteAllActions(null);
			done = true;
		}
	}
}