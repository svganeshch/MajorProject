using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementManager : CharacterMovementManager
{
    public Transform dest;

    float horizontalInput;
    float verticalInput;
    float moveAmount;

    protected Vector3 targetRotationDirection;
    protected Quaternion targetRotation;
    protected Quaternion finalRotation;

    Enemy enemy;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
    }

    protected override void Start()
    {
        base.Start();

        //enemy.navMeshAgent.updatePosition = false;
        //enemy.navMeshAgent.updateRotation = false;
    }

    protected override void Update()
    {
        base.Update();

        enemy.navMeshAgent.SetDestination(dest.position);
    }

    protected override void HandleGroundedMovement()
    {
        if (!enemy.canMove) return;

        enemy.controller.Move(enemy.moveSpeed * Time.deltaTime * enemy.navMeshAgent.desiredVelocity.normalized);

        enemy.navMeshAgent.nextPosition = enemy.transform.position;
        enemy.navMeshAgent.velocity = enemy.controller.velocity;
    }

    protected override void HandleCharacterAnimation()
    {
        horizontalInput = enemy.controller.velocity.x;
        verticalInput = enemy.controller.velocity.z;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        enemy.characterAnimatorManager.SetAnimatorParameters(0, moveAmount);
    }

    protected override void HandleCharacterRotation()
    {
        if (!enemy.canRotate) return;

        targetRotationDirection = Vector3.forward * verticalInput;
        targetRotationDirection += Vector3.right * horizontalInput;
        targetRotationDirection.y = 0f;
        targetRotationDirection.Normalize();

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = character.transform.forward;
        }

        targetRotation = Quaternion.LookRotation(targetRotationDirection);

        finalRotation = Quaternion.Slerp(character.transform.rotation, targetRotation, character.rotationDampTime * Time.deltaTime);
        character.transform.rotation = finalRotation;
    }
}
