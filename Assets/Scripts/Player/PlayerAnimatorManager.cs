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
}
