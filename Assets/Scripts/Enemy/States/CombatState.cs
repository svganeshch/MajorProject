using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    public CombatState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
    }

    [Header("Attacks")]
    public List<EnemyAttackAction> enemyAttacks;
    protected List<EnemyAttackAction> potentialAttacks;
    private EnemyAttackAction selectedAttack;
    private EnemyAttackAction previousAttack;
    protected bool hasAttack = false;
    
    [Header("Combo")]
    protected bool canPerformCombo = false;
    protected int comboChance = 25;
    protected bool hasRolledForComboChance = false;
    
    [Header("Engage Distance")]
    public float engagementDistance = 2.5f;

    public override void Enter()
    {
        base.Enter();

        enemyAttacks = enemyCharacter.enemyCombatManager.enemyAttacks;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        if (enemyCharacter.performingAction) return;
        
        enemyCharacter.enemyCombatManager.RotateTowardsTarget();

        if (enemyCharacter.enemyCombatManager.currentTarget == null)
        {
            stateMachine.ChangeState(enemyCharacter.idleState);
        }
        
        if (enemyCharacter.enemyCombatManager.distanceToTarget > engagementDistance)
        {
            stateMachine.ChangeState(enemyCharacter.pursueState);
        }
        
        if (!hasAttack)
            GetNewAttack(enemyCharacter);
        else
        {
            enemyCharacter.attackState.currentAttack = selectedAttack;
            stateMachine.ChangeState(enemyCharacter.attackState);
        }
        
        enemyCharacter.SetNavMeshAgentDestination();
    }

    protected virtual void GetNewAttack(Enemy enemy)
    {
        potentialAttacks = new List<EnemyAttackAction>();

        foreach (var attack in enemyAttacks)
        {
            if (attack.minimumAttackDistance > enemy.enemyCombatManager.distanceToTarget)
                continue;

            if (attack.maximumAttackDistance < enemy.enemyCombatManager.distanceToTarget)
                continue;

            if (attack.minimumAttackAngle > enemy.enemyCombatManager.enemyFOV)
                continue;

            if (attack.maximumAttackAngle < enemy.enemyCombatManager.enemyFOV)
                continue;

            potentialAttacks.Add(attack);
        }
        
        if (potentialAttacks.Count <= 0)
            return;

        var totalWeight = 0;
        foreach (var attack in potentialAttacks)
        {
            totalWeight += attack.attackWeight;
        }
        
        var randomWeight = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;

            if (randomWeight <= processedWeight)
            {
                selectedAttack = attack;
                previousAttack = selectedAttack;
                hasAttack = true;
                
                return;
            }
        }
    }

    protected virtual bool RollForCombo(int outcomeChance)
    {
        bool outComeChanceResult = false;
        
        int roll = UnityEngine.Random.Range(0, 100);
        
        if (roll < outcomeChance)
            outComeChanceResult = true;
        
        return outComeChanceResult;
    }

    public override void Exit()
    {
        base.Exit();
        
        hasRolledForComboChance = false;
        hasAttack = false;
    }
}