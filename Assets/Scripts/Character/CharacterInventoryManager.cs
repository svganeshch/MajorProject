using System;
using UnityEngine;

public class CharacterInventoryManager : MonoBehaviour
{
    public WeaponItem currentRightHandWeapon;
    public WeaponItem currentLeftHandWeapon;

    protected virtual void Awake()
    {
        // Instantiate a clone WeaponItem scriptable object
        var rightWeaponItem = Instantiate(currentRightHandWeapon);
        currentRightHandWeapon = rightWeaponItem;
    }
}
