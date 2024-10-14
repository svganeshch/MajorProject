using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : CharacterAnimatorManager
{
    Enemy enemy;
    Transform enemyTransform;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
        enemyTransform = enemy.transform;
    }

    private void OnAnimatorMove()
    {
        if (enemy.applyRootMotion)
        {
            Vector3 velocity = enemy.animator.deltaPosition;

            enemy.controller.Move(velocity);
            enemyTransform.rotation *= enemy.animator.deltaRotation;
        }
    }
}
