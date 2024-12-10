using System;
using UnityEngine;

public class EnemyCombatManager : CharacterCombatManager
{
    [Header("Target detection settings")]
    public float detectionRadius = 15;
    public float minimumDetectionAngle = -35;
    public float maximumDetectionAngle = 35;
    
    public void FindTarget(Enemy enemy)
    {
        if (currentTarget != null) return;
        
        Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, detectionRadius, LayerMaskManager.instance.characterLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            Character targetCharacter = colliders[i].gameObject.GetComponent<Character>();
            
            if (targetCharacter == null ) continue;
            
            if (!targetCharacter.CompareTag("Player")) continue;
            
            if (targetCharacter == enemy) continue;
            
            if (targetCharacter.isDead) continue;
            
            Vector3 targetDirection = targetCharacter.transform.position - enemy.transform.position;
            float viewAngle = Vector3.Angle(targetDirection, enemy.transform.forward);

            if (viewAngle > minimumDetectionAngle && viewAngle < maximumDetectionAngle)
            {
                if (Physics.Linecast(enemy.characterCombatManager.lockOnTransform.transform.position,
                        targetCharacter.characterCombatManager.lockOnTransform.transform.position, LayerMaskManager.instance.obstacleLayer))
                {
                    /*Debug.DrawLine(enemy.characterCombatManager.lockOnTransform.transform.position,
                        targetCharacter.characterCombatManager.lockOnTransform.transform.position);
                    Debug.Log("view to target is blocked!!");*/
                }
                else
                {
                    currentTarget = targetCharacter;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}