using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
    Player player;

    Vector3 jumpDirection;
    Vector3 freefallDirection;

    float jumpHeight;

    public JumpState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        player = character as Player;
    }

    public override void Enter()
    {
        base.Enter();

        jumpDirection = player.transform.forward;
        SetJumpDirectionVelocity();

        player.playerAnimatorManager.PlayJumpAction();

        jumpHeight = Mathf.Sqrt(player.jumpHeight * -2 * player.characterMovementManager.gravityForce) / 20;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        freefallDirection = jumpDirection;
        player.controller.Move((player.jumpForwardVelocity * jumpDirection + player.freeFallControlVelocity * freefallDirection) * Time.deltaTime);
    }

    private void SetJumpDirectionVelocity()
    {
        if (player.playerMovementManager.moveAmount <= 0.5f)
        {
            jumpDirection *= 0.25f;
            Debug.Log("normal jump");
        }
    }

    public override void Exit()
    {
        base.Exit();

        //Debug.Log("jump height" + Mathf.Sqrt(player.jumpHeight * -2 * player.characterMovementManager.gravityForce) / 20);
    }
}
