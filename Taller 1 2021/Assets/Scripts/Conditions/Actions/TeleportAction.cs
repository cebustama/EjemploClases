using UnityEngine;
using System.Collections;

[AddComponentMenu("Playground/Actions/Teleport")]
[ExecuteInEditMode]
public class TeleportAction : Action
{
	public GameObject objectToMove;
	public bool objectThatCollided = false;

	public Transform newPositionObject;
	public Vector2 newPosition;
	public bool stopMovements = true;


    private void Update()
    {
        if (!Application.isPlaying)
        {
			if (newPositionObject != null) newPosition = newPositionObject.transform.position;
        }
    }


    // Moves the GameObject instantly to a custom position
    public override bool ExecuteAction(GameObject dataObject)
	{
		Rigidbody2D rb2D;

		Vector2 finalPos = (newPositionObject == null) ? newPosition : (Vector2) newPositionObject.transform.position;

		if(objectToMove != null)
		{
			//moves the specified object
			objectToMove.transform.position = newPosition;
			rb2D = objectToMove.GetComponent<Rigidbody2D>();
		}
		else if (objectThatCollided)
        {
			dataObject.transform.position = newPosition;
			rb2D = dataObject.GetComponent<Rigidbody2D>();
		}
		else
		{
			//moves this object
			transform.position = newPosition;
			rb2D = transform.GetComponent<Rigidbody2D>();
		}


		//in case the object has physics, we can bring it to an halt
		if(stopMovements
			&& rb2D != null)
		{
			rb2D.velocity = Vector3.zero;
			rb2D.angularVelocity = 0f;
		}

		return true;
	}
}
