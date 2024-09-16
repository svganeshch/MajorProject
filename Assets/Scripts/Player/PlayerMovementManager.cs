using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovementManager : CharacterMovementManager
{
    Player player;

    float horizontalInput;
    float verticalInput;
    float moveAmount;
    Vector3 moveDirection;

    protected Vector3 targetRotationDirection;
    protected Quaternion targetRotation;
    protected Quaternion finalRotation;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    protected override void Update()
    {
        base.Update();

        horizontalInput = player.inputManager.horizontalInput;
        verticalInput = player.inputManager.verticalInput;

        moveDirection = Vector3.forward * verticalInput;
        moveDirection += Vector3.right * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        player.controller.Move(player.moveSpeed * Time.deltaTime * moveDirection);

        HandlePlayerAnimation();
        HandleRotation();
    }

    private void HandlePlayerAnimation()
    {
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5f && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        player.characterAnimatorManager.SetAnimatorParameters(0, moveAmount);
    }

    protected virtual void HandleRotation()
    {
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
