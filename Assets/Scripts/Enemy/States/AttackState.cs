public class AttackState : State
{
    public AttackState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
    }

    public EnemyAttackAction currentAttack;
    public bool willPerformCombo = false;
    
    public bool hasPerformedAttack = false;
    public bool hasPerformedCombo = false;

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemyCharacter.enemyCombatManager.currentTarget == null)
        {
            stateMachine.ChangeState(enemyCharacter.idleState);
        }
        
        if (enemyCharacter.enemyCombatManager.currentTarget.isDead)
        {
            stateMachine.ChangeState(enemyCharacter.idleState);
        }
        
        enemyCharacter.enemyCombatManager.RotateTowardsTargetWhileAttacking();
        enemyCharacter.enemyAnimatorManager.SetAnimatorParameters(0, 0);

        if (willPerformCombo && !hasPerformedCombo)
        {
            if (currentAttack.comboAction != null)
            {
                hasPerformedCombo = true;
                currentAttack.comboAction.AttemptToPerformAction(enemyCharacter);
            }
        }
        
        if (enemyCharacter.performingAction)
            return;
        
        if (!hasPerformedAttack)
        {
            if (enemyCharacter.enemyCombatManager.actionRecoveryTimer > 0)
                return;
            
            PerformedAttack(enemyCharacter);
            
            return;
        }
        
        stateMachine.ChangeState(enemyCharacter.combatState);
    }

    protected void PerformedAttack(Enemy enemy)
    {
        hasPerformedAttack = true;
        currentAttack.AttemptToPerformAction(enemy);

        enemy.enemyCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
    }

    public override void Exit()
    {
        base.Exit();
        
        hasPerformedAttack = false;
        hasPerformedCombo = false;
    }
}