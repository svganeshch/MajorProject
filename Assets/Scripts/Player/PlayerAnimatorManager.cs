using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    Player player;
    Transform playerTransform;

    private static readonly int jumpHash = Animator.StringToHash("jump");

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
        playerTransform = player.transform;
    }

    private void OnAnimatorMove()
    {
        if (player.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;

            player.controller.Move(velocity);
            playerTransform.rotation *= player.animator.deltaRotation;
        }
    }

    public void PlayJumpAction()
    {
        PlayCharacterActionAnimation(jumpHash, true, false, false, false);
    }

    public void PlayEffect(string effect)
    {
        WorldCharacterEffectsManager.Instance.PlayEffect(effect);
    }

    public void StopEffect(string effect)
    {
        WorldCharacterEffectsManager.Instance.StopEffect(effect);
    }

    public void EnableDamageCollider()
    {
        player.playerEquipmentManager.rightWeaponManager.swordDamageCollider.EnableDamageCollider();
        WorldCharacterEffectsManager.Instance.PlayEffect("sword");
    }

    public void DisableDamageCollider()
    {
        player.playerEquipmentManager.rightWeaponManager.swordDamageCollider.DisableDamageCollider();
        WorldCharacterEffectsManager.Instance.StopEffect("sword");
    }

    public void EnableCombo()
    {
        player.canCombo = true;
    }

    public void DisableCombo()
    {
        player.canCombo = false;
    }
}
