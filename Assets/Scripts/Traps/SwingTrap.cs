using System;
using UnityEngine;

public class SwingTrap : MonoBehaviour, IZoneItem
{
    [Header("Swing Settings")]
    public float swingAngle = 45f;
    public float swingSpeed = 2f;

    private DamageCollider damageCollider;
    private Rigidbody rb;
    private Quaternion initialRotation;

    private float previousSwingAngle = 0f;
    
    private bool swinging;

    private void Awake()
    {
        damageCollider = GetComponentInChildren<DamageCollider>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        initialRotation = transform.rotation;
    }
    
    public void EnableZoneItem()
    {
        swinging = true;
    }

    public void DisableZoneItem()
    {
        swinging = false;
    }

    private void FixedUpdate()
    {
        if (!swinging) return;
        
        damageCollider.EnableDamageCollider();
        
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(angle, 0, 0);

        rb.MoveRotation(targetRotation);
        
        if (Math.Abs(Mathf.Sign(angle) - Mathf.Sign(previousSwingAngle)) > 0.1f)
        {
            damageCollider.DisableDamageCollider();
        }

        previousSwingAngle = angle;
    }
}