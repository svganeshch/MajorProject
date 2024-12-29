using System;
using UnityEngine;

public class PendulumTrap : MonoBehaviour, IZoneItem
{
    private HingeJoint _hingeJoint;
    private Rigidbody connectedRb;
    
    private Vector3 lastAngularVelocity;
    private float lastDirection;
    private bool switchedDir = false;

    private void Awake()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        connectedRb = _hingeJoint.connectedBody;
    }

    private void FixedUpdate()
    {
        Vector3 currentAngularVelocity = connectedRb.angularVelocity;

        // Check if direction has changed on any axis
        if (Vector3.Dot(lastAngularVelocity, currentAngularVelocity) < 0 && currentAngularVelocity.magnitude > 0.01f)
        {
            Debug.Log("Pendulum changed direction!");
        }

        // Update the last angular velocity for next frame comparison
        lastAngularVelocity = currentAngularVelocity;
    }

    public void EnableZoneItem()
    {
        gameObject.SetActive(true);
    }

    public void DisableZoneItem()
    {
        gameObject.SetActive(false);
    }
}
