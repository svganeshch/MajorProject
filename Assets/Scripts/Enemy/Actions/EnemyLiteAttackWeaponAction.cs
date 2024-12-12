using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLiteAttackAction", menuName = "Actions / Weapon Actions / Enemy / Lite Attack Action")]
public class EnemyLiteAttackWeaponAction : EnemyAttackAction
{
    public override void AttemptToPerformAction(Character characterPerformingAction)
    {
        base.AttemptToPerformAction(characterPerformingAction);
        
        // check for stamina and return
        
        if (!characterPerformingAction.isGrounded) return;
        
        PerformLiteAttack(characterPerformingAction);
    }

    private void PerformLiteAttack(Character characterPerformingAction)
    {
        characterPerformingAction.characterAnimatorManager.PlayCharacterActionAnimation(Animator.StringToHash(attackAnimation), true);
    }
}