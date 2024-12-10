using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public abstract class CharacterMovementManager : MonoBehaviour
{
    [HideInInspector] public Character character;
    
    [Header("Movement Settings")]
    protected float horizontalInput;
    protected float verticalInput;
    protected float moveAmount;
    protected Vector3 moveDirection;
    
    [Header("Rotation Settings")]
    protected Vector3 targetRotationDirection;
    protected Quaternion targetRotation;
    protected Quaternion finalRotation;

    [Header("Gravity Settings")]
    protected Vector3 yVelocity;
    protected float gravityForce = -40;
    protected float groundCheckSphereRadius = 0.3f;
    protected float groundedYVelocity = -20;
    protected float fallStartYVelocity = -5;
    protected float inAirTime = 0;
    protected bool fallingVelocitySet = false;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        GetMovementInputs();
        HandleGroundedMovement();
        HandleCharacterAnimation();
        HandleCharacterRotation();
        HandleGroundCheck();
    }
    
    protected abstract void GetMovementInputs();

    protected virtual void HandleGroundedMovement()
    {
        if (!character.canMove) return;
        
        moveDirection = Vector3.forward * verticalInput;
        moveDirection += Vector3.right * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

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
    }

    protected virtual void HandleCharacterAnimation()
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

        character.characterAnimatorManager.SetAnimatorParameters(0, moveAmount);
    }

    protected virtual void HandleCharacterRotation()
    {
        if (!character.canRotate) return;
        
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

    protected virtual void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, LayerMaskManager.instance.groundLayer);

        character.characterAnimatorManager.IsGrounded = character.isGrounded;

        if (character.isGrounded)
        {
            if (yVelocity.y < 0f)
            {
                inAirTime = 0;
                fallingVelocitySet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if (!character.isJumping && !fallingVelocitySet)
            {
                fallingVelocitySet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTime += Time.deltaTime;
            character.characterAnimatorManager.InAirTime = inAirTime;

            yVelocity.y += gravityForce * Time.deltaTime;
        }

        if (character.performingParkour) return;
        character.controller.Move(yVelocity * Time.deltaTime);
    }
}
