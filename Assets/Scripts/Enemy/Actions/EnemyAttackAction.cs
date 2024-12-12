using UnityEngine;

public class EnemyAttackAction : WeaponItemAction
{
        [Header("Attack")]
        [SerializeField] protected string attackAnimation;
        
        [Header("Combo Action")]
        public EnemyAttackAction comboAction;

        [Header("Action Values")]
        public int attackWeight = 50;
        public float actionRecoveryTime = 1.5f;
        public float minimumAttackAngle = -35f;
        public float maximumAttackAngle = 35f;
        public float minimumAttackDistance = 0f;
        public float maximumAttackDistance = 5f;

        public override void AttemptToPerformAction(Character characterPerformingAction)
        {
                base.AttemptToPerformAction(characterPerformingAction);
        }
}
