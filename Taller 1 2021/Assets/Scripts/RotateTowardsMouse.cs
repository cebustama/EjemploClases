using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsMouse : MonoBehaviour
{
    public float angleOffset = 0f;

    protected float angle;

    void Update()
    {
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        angle = Vector2.SignedAngle(Vector2.right, dir) + angleOffset;
        transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
