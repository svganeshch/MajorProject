using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementManager : CharacterMovementManager
{
    Enemy enemy;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
    }

    protected override void GetMovementInputs()
    {
        horizontalInput = enemy.controller.velocity.x;
        verticalInput = enemy.controller.velocity.z;
    }

    protected override void HandleGroundedMovement()
    {
        if (!character.canMove) return;
        
        moveDirection = enemy.navMeshAgent.desiredVelocity.normalized;

        if (character.isSprinting)
        {
            character.controller.Move(character.sprintingSpeed * Time.deltaTime * moveDirection);
        }
        else
        {
            if (moveAmount > 0.5f)
            {
                character.controller.Move(character.runningSpeed * Time.deltaTime * moveDirection);
            }
            else if (moveAmount <= 0.5f)
            {
                character.controller.Move(character.walkingSpeed * Time.deltaTime * moveDirection);
            }
        }
        
        enemy.navMeshAgent.nextPosition = enemy.transform.position;
        enemy.navMeshAgent.velocity = enemy.controller.velocity;
    }

    protected override void HandleCharacterAnimation()
    {
        horizontalInput = enemy.controller.velocity.x;
        verticalInput = enemy.controller.velocity.z;

        base.HandleCharacterAnimation();
    }
}
