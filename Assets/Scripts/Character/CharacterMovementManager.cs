using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CharacterMovementManager : MonoBehaviour
{
    [HideInInspector] public Character character;

    public bool isGrounded;
    [HideInInspector] public Vector3 yVelocity;
    [HideInInspector] public float gravityForce = -40;
    protected float groundCheckSphereRadius = 0.3f;
    protected float groundedYVelocity = -20;
    protected float fallStartYVelocity = -5;
    [HideInInspector] public float inAirTime = 0;
    protected bool fallingVelocitySet = false;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();
    }

    protected virtual void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, LayerMaskManager.instance.groundLayer);

        character.characterAnimatorManager.IsGrounded = isGrounded;

        if (isGrounded)
        {
            if (yVelocity.y < 0f)
            {
                inAirTime = 0;
                fallingVelocitySet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        //else
        //{
        //    if (character.characterStateMachine.currentState != character.jumpState && !fallingVelocitySet)
        //    {
        //        fallingVelocitySet = true;
        //        yVelocity.y = fallStartYVelocity;
        //    }

        //    inAirTime += Time.deltaTime;
        //    character.characterAnimatorManager.InAirTime = inAirTime;

        //    yVelocity.y += gravityForce * Time.deltaTime;
        //}

        character.controller.Move(yVelocity * Time.deltaTime);
    }
}
