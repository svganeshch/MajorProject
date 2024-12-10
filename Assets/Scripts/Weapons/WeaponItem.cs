using UnityEngine;

public class WeaponItem : ScriptableObject
{
    [Header("Animator override Controller")]
    public AnimatorOverrideController overrideController;

    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Damage")]
    public int physicalDamage = 0;
    public int lightingDamage = 0;

    [Header("Actions")]
    public WeaponItemAction liteAttackAction;
    public WeaponItemAction heavyAttackAction;

    [Header("Weapon SFX")]
    public AudioClip slashSfx;
    public AudioClip blockSfx;

    [Header("Weapon VFX")]
    public ParticleSystem slashVfx;
}
