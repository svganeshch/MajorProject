using System;
using UnityEngine;
public class SpikeTrap : MonoBehaviour, IZoneItem
{
    private static readonly int TrapSpeedHash = Animator.StringToHash("trapSpeed");
    public float trapSpeed = 1f;
    
    Animator animator;
    DamageCollider[] spikeDamageColliders;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spikeDamageColliders = GetComponentsInChildren<DamageCollider>();
    }

    private void Start()
    {
        animator.SetFloat(TrapSpeedHash, trapSpeed);
    }

    public void EnableZoneItem()
    {
        gameObject.SetActive(true);
    }

    public void DisableZoneItem()
    {
        gameObject.SetActive(false);
    }

    public void EnableDamageCollider()
    {
        foreach (var damageCollider in spikeDamageColliders)
        {
            damageCollider.EnableDamageCollider();
        }
    }

    public void DisableDamageCollider()
    {
        foreach (var damageCollider in spikeDamageColliders)
        {
            damageCollider.DisableDamageCollider();
        }
    }
}