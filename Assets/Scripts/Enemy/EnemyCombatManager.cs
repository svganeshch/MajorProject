using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatManager : CharacterCombatManager
{
    private Enemy _enemy;

    [Header("Target info")]
    public float distanceToTarget;
    public float enemyFOV;
    
    [Header("Target detection settings")]
    public float detectionRadius = 15;
    public float minimumDetectionAngle = -35;
    public float maximumDetectionAngle = 35;

    [Header("Attacks Info")]
    public List<EnemyAttackAction> enemyAttacks;
    public float attackRotationSpeed = 30f;
    public float actionRecoveryTimer = 0;

    protected override void Awake()
    {
        base.Awake();

        _enemy = GetComponent<Enemy>();
    }

    protected override void Update()
    {
        base.Update();
        
        HandleActionRecovery(_enemy);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (currentTarget != null)
        {
            enemyFOV = GetAngleOfTarget();
            distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        }
    }

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

    public void RotateTowardsTarget()
    {
        Vector3 direction = currentTarget.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        
        if (direction == Vector3.zero)
            direction = _enemy.transform.forward;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _enemy.transform.rotation = targetRotation;
    }

    public void RotateTowardsTargetWhileAttacking()
    {
        if (currentTarget == null) return;
        
        if (!_enemy.canRotate) return;

        if (!_enemy.performingAction) return;
        
        Vector3 direction = currentTarget.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        
        if (direction == Vector3.zero)
            direction = _enemy.transform.forward;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
    }

    private void HandleActionRecovery(Enemy enemy)
    {
        if (!(actionRecoveryTimer > 0)) return;
        if (!enemy.performingAction)
        {
            actionRecoveryTimer -= Time.deltaTime;
        }
    }

    private float GetAngleOfTarget()
    {
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        targetDirection.y = 0;
        
        float viewAngle = Vector3.Angle(transform.forward, targetDirection);
        Vector3 cross = Vector3.Cross(transform.forward, targetDirection);
        
        if (cross.y < 0) viewAngle = -viewAngle;
        
        return viewAngle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}