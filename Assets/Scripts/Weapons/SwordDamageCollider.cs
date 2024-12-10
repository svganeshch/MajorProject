using UnityEngine;

public class SwordDamageCollider : DamageCollider
{
    protected override void Awake()
    {
        base.Awake();
        
        damageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Character damageTarget = other.GetComponentInParent<Character>();

        if (damageTarget != null )
        {
            if (damageTarget == characterCausingDamage) return;
            
            DamageTarget(damageTarget);
        }
    }
}
