using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public SwordDamageCollider swordDamageCollider;

    private void Awake()
    {
        swordDamageCollider = GetComponentInChildren<SwordDamageCollider>();
    }

    public void SetWeaponDamage(Character characterWeildingWeapon, WeaponItem weaponItem)
    {
        swordDamageCollider.characterCausingDamage = characterWeildingWeapon;
        swordDamageCollider.physicalDamage = weaponItem.physicalDamage;
        swordDamageCollider.lightingDamage = weaponItem.lightingDamage;
    }
}
