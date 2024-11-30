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
        FXManager.Instance.PlayEffect(effect);
    }

    public void StopEffect(string effect)
    {
        FXManager.Instance.StopEffect(effect);
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
