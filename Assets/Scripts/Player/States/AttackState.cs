using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    Player player;

    protected bool liteAttack;

    bool isCombo;
    bool dodge;
    bool block;

    public AttackState(Character _character, StateMachine _stateMachine, bool liteAttack) : base(_character, _stateMachine)
    {
        player = character as Player;
        this.liteAttack = liteAttack;
    }

    public override void Enter()
    {
        base.Enter();

        isCombo = false;
        dodge = false;
        block = false;
        
        if (player.performingAction)
        {
            if (!player.canCombo) stateMachine.ChangeState(player.idleState);
            
            player.canCombo = false;
            isCombo = true;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //if (player.canPerformAction)
        //{
        //    //HandleRotation();

        //    if (dodge)
        //    {
        //        player.canPerformAction = false;
        //        dodge = false;
        //        stateMachine.ChangeState(player.dodgeState);
        //    }

        //    if (block)
        //    {
        //        player.canPerformAction = false;
        //        block = false;
        //        stateMachine.ChangeState(player.blockState);
        //    }
        //}
        
        player.playerAnimatorManager.PlayAttackAction(this, isCombo, true);

        isCombo = false;

        Debug.Log("attack triggered");

        stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        //liteAttack = false;
    }
}
