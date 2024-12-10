using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemyCharacter.characterCombatManager.currentTarget != null)
        {
            enemyCharacter.enemyMovementManager.UpdateNavTarget(enemyCharacter.characterCombatManager.currentTarget.transform.position);
        }
        else
        {
            enemyCharacter.enemyCombatManager.FindTarget(enemyCharacter);
        }
    }
}
