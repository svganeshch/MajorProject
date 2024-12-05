using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Hit Damage")]
public class HitDamageEffect : InstantCharacterEffect
{
    Character characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float lightingDamage = 0;

    private int finalDamageDealt = 0;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public string damageAnimation;

    [Header("SoundFX")]
    public bool willPlayDamageSFX = true;

    public override void ProcessEffect(Character character)
    {
        base.ProcessEffect(character);

        if (character.isDead) return;

        PlayHitAnimation(character);
        CalculateHitDamage(character);
    }

    private void CalculateHitDamage(Character character)
    {
        finalDamageDealt = Mathf.RoundToInt(physicalDamage + lightingDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        character.characterHealthManager.TakeDamage(finalDamageDealt);
    }

    private void PlayHitAnimation(Character character)
    {
        character.characterAnimatorManager.PlayHitAnimation();
    }
}
