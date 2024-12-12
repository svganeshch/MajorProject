using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterAnimatorManager : MonoBehaviour
{
    Character character;

    private int previousActionHash;

    private static readonly int speedX = Animator.StringToHash("speedX");
    private static readonly int speedY = Animator.StringToHash("speedY");
    
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");
    private static readonly int isGroundedHash = Animator.StringToHash("isGrounded");
    private static readonly int inAirTimeHash = Animator.StringToHash("inAirTime");

    private int[] LiteAttack =
    {
        Animator.StringToHash("lite_attack1"),
        Animator.StringToHash("lite_attack2"),
        Animator.StringToHash("lite_attack3")
    };

    private int[] HeavyAttack =
    {
        Animator.StringToHash("heavy_attack1"),
        Animator.StringToHash("heavy_attack2")
    };

    private static readonly int forwardDash = Animator.StringToHash("Forward_Dash");
    private static readonly int backwardDash = Animator.StringToHash("Backward_Dash");

    private static readonly int hitHash = Animator.StringToHash("Hit_B");
    private static readonly int deathHash = Animator.StringToHash("Death");
    
    private static readonly int climbHash = Animator.StringToHash("Climb");
    private static readonly int vaultHash = Animator.StringToHash("Vault");

    public bool IsMoving
    {
        get => character.animator.GetBool(isMovingHash);

        set => character.animator.SetBool(isMovingHash, value);
    }

    public bool IsGrounded
    {
        get => character.animator.GetBool(isGroundedHash);
        set => character.animator.SetBool(isGroundedHash, value);
    }

    public float InAirTime
    {
        get => character.animator.GetFloat(inAirTimeHash);
        set => character.animator.SetFloat(inAirTimeHash, value);
    }

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }

    public void SetAnimatorParameters(float horizontalInput, float verticalInput)
    {
        float horizontal = horizontalInput;
        float vertical = verticalInput;

        if (character.isSprinting)
        {
            vertical = 2;
        }
        
        character.animator.SetFloat(speedX, horizontal, character.speedDampTime, Time.deltaTime);
        character.animator.SetFloat(speedY, vertical, character.speedDampTime, Time.deltaTime);
    }

    public virtual void PlayCharacterActionAnimation(
        int animationClipHash,
        bool isPerformingAction = true,
        bool canRotate = false,
        bool canMove = false,
        bool applyRootMotion = true)
    {
        previousActionHash = animationClipHash;

        character.animator.CrossFade(animationClipHash, character.animationFadeTime);

        character.performingAction = isPerformingAction;
        character.applyRootMotion = applyRootMotion;
        character.canRotate = canRotate;
        character.canMove = canMove;
    }

    public void PlayAttackAction(WeaponItemAction weaponAction, bool canCombo, bool canRotate = false)
    {
        int[] attackHashes = new int[LiteAttack.Length];

        if (weaponAction == character.CharacterInventoryManager.currentRightHandWeapon.liteAttackAction)
        {
            attackHashes = LiteAttack;
        }
        else if (weaponAction == character.CharacterInventoryManager.currentRightHandWeapon.heavyAttackAction)
        {
            attackHashes = HeavyAttack;
        }

        int nextAttackHash = attackHashes[0];

        if (canCombo)
        {
            if (previousActionHash == attackHashes[0])
                nextAttackHash = attackHashes[1];
            else if (previousActionHash == attackHashes[1])
                nextAttackHash = attackHashes[0];
        }

        PlayCharacterActionAnimation(nextAttackHash, true, canRotate);
    }

    public void PlayForwardDash()
    {
        PlayCharacterActionAnimation(forwardDash, true);
    }

    public void PlayBackwardDash()
    {
        PlayCharacterActionAnimation(backwardDash, true);
    }

    public void PlayHitAnimation()
    {
        PlayCharacterActionAnimation(hitHash, true);
    }

    public void PlayDeathAction()
    {
        PlayCharacterActionAnimation(deathHash, true);
    }

    public void PlayParkourAction(ParkourActionAnimation parkourActionAnimation)
    {
        var parkourActionHash = parkourActionAnimation switch
        {
            ParkourActionAnimation.Vault => vaultHash,
            ParkourActionAnimation.Climb => climbHash,
            _ => 0
        };

        PlayCharacterActionAnimation(parkourActionHash, true);
    }
    
    // Animation Events
    public void PlayEffect(string effect)
    {
        WorldCharacterEffectsManager.Instance.PlayEffect(effect);
    }

    public void StopEffect(string effect)
    {
        WorldCharacterEffectsManager.Instance.StopEffect(effect);
    }

    public virtual void EnableCanRotate() {}

    public virtual void DisableCanRotate() {}

    public void EnableDamageCollider()
    {
        character.characterEquipmentManager.rightWeaponManager.swordDamageCollider.EnableDamageCollider();
        WorldCharacterEffectsManager.Instance.PlayWeaponSlashEffect(character.CharacterInventoryManager.currentRightHandWeapon.slashVfx, true);
    }

    public void DisableDamageCollider()
    {
        character.characterEquipmentManager.rightWeaponManager.swordDamageCollider.DisableDamageCollider();
        WorldCharacterEffectsManager.Instance.PlayWeaponSlashEffect(character.CharacterInventoryManager.currentRightHandWeapon.slashVfx, false);
    }

    public void EnableCombo()
    {
        character.canCombo = true;
    }

    public void DisableCombo()
    {
        character.canCombo = false;
    }
}
