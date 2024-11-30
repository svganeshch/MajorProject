using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public abstract class CharacterMovementManager : MonoBehaviour
{
    [HideInInspector] public Character character;

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
        HandleGroundedMovement();
        HandleCharacterAnimation();
        HandleCharacterRotation();
        HandleGroundCheck();
    }

    protected abstract void HandleGroundedMovement();
    protected abstract void HandleCharacterAnimation();

    protected abstract void HandleCharacterRotation();

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

        character.controller.Move(yVelocity * Time.deltaTime);
    }
}
