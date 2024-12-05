using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider damageCollider;
    protected List<Character> damagedCharacters = new List<Character>();

    public Character characterCausingDamage;
    public int physicalDamage = 0;
    public int lightingDamage = 0;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Character damageTarget = other.GetComponentInParent<Character>();

        if (damageTarget != null )
        {
            DamageTarget(damageTarget);
        }
    }

    private void DamageTarget(Character damageCharacter)
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
