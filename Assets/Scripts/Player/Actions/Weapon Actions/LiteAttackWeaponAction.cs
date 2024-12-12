using UnityEngine;

[CreateAssetMenu(fileName = "LiteAttackAction", menuName = "Actions / Weapon Actions / Lite Attack Action")]
public class LiteAttackWeaponAction : WeaponItemAction
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
        if (characterPerformingAction.performingAction && characterPerformingAction.canCombo)
        {
            characterPerformingAction.canCombo = false;
            characterPerformingAction.characterAnimatorManager.PlayAttackAction(this, true);
        }
        else if (!characterPerformingAction.performingAction)
        {
            characterPerformingAction.characterAnimatorManager.PlayAttackAction(this, false);
        }
    }
}
