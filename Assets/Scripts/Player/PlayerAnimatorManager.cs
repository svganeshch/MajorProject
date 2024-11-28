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
        if (effect == "dash")
        {
            FXManager.Instance.forwardDashEffect.Play();
            return;
        }

        FXManager.Instance.weaponSlashEffect.Play();
    }

    public void StopEffect(string effect)
    {
        if (effect == "dash")
        {
            FXManager.Instance.forwardDashEffect.Stop();
            return;
        }

        FXManager.Instance.weaponSlashEffect.Stop();
    }

    public void EnableCombo()
    {
        player.canCombo = true;
    }

    public void DisableCombo()
    {
        player.canCombo = false;
    }

    public void ApplyJumpVelocity()
    {
        player.characterMovementManager.yVelocity.y = Mathf.Sqrt(player.jumpHeight * -2 * player.characterMovementManager.gravityForce);
    }
}
