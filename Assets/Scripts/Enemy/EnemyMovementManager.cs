using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementManager : CharacterMovementManager
{
    public void RotateTowardsAgent(Enemy enemy)
    {
        enemy.transform.rotation = enemy.navMeshAgent.transform.rotation;
    }
}
