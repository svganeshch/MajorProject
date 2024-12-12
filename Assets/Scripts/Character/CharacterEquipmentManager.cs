using System;
using UnityEngine;

public class CharacterEquipmentManager : MonoBehaviour
{
    Character character;
    
    public WeaponInstantiationSlot rightHandSlot;
    public WeaponInstantiationSlot leftHandSlot;

    public WeaponManager rightWeaponManager;
    public WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();

        InitializeWeaponSlots();
    }

    protected virtual void Start()
    {
        LoadWeaponsOnBothHands();
    }

    private void InitializeWeaponSlots()
    {
        WeaponInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    private void LoadRightWeapon()
    {
        if (character.CharacterInventoryManager.currentRightHandWeapon != null)
        {
            // Instantitate weapon model
            rightHandWeaponModel = Instantiate(character.CharacterInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(character, character.CharacterInventoryManager.currentRightHandWeapon);
            
            // Instantiate weapon vfx
            var weaponSlash = Instantiate(character.CharacterInventoryManager.currentRightHandWeapon.slashVfx, rightHandSlot.transform);
            character.CharacterInventoryManager.currentRightHandWeapon.slashVfx = weaponSlash;
        }
    }

    private void LoadLeftWeapon()
    {
        if (character.CharacterInventoryManager.currentLeftHandWeapon != null)
        {
            leftHandWeaponModel = Instantiate(character.CharacterInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(character, character.CharacterInventoryManager.currentLeftHandWeapon);
        }
    }
}
