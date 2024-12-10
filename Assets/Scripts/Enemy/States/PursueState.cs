public class PursueState : State
{
    public PursueState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        if (enemyCharacter.performingAction) return;

        if (enemyCharacter.enemyCombatManager.currentTarget == null)
        {
            stateMachine.ChangeState(enemyCharacter.idleState);
        }
        
        enemyCharacter.navMeshAgent.SetDestination(enemyCharacter.characterCombatManager.currentTarget.transform.position);
    }
}