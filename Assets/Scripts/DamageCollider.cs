using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    protected Collider damageCollider;
    protected List<Character> damagedCharacters = new List<Character>();

    [HideInInspector] public Character characterCausingDamage;
    public int physicalDamage = 0;
    public int lightingDamage = 0;

    protected virtual void Awake()
    {
        damageCollider = GetComponent<Collider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Character damageTarget = other.GetComponentInParent<Character>();

        if (damageTarget != null )
        {
            DamageTarget(damageTarget);
        }
    }

    protected void DamageTarget(Character damageCharacter)
    {
        if (damagedCharacters.Contains(damageCharacter)) return;

        damagedCharacters.Add(damageCharacter);

        HitDamageEffect hitDamageEffect = Instantiate(WorldCharacterEffectsManager.Instance.hitDamageEffect, damageCharacter.transform);
        hitDamageEffect.physicalDamage = physicalDamage;
        hitDamageEffect.lightingDamage = lightingDamage;

        damageCharacter.characterEffectsManager.ProcessInstantEffect(hitDamageEffect);
    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        damagedCharacters.Clear();
    }
}
