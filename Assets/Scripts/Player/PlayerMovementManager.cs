using UnityEngine;

public class PlayerMovementManager : CharacterMovementManager
{
    Transform mainCamTransform;
    private Vector3 camForward;
    private Vector3 camRight;
    Player player;
    
    [Header("Movement Settings")]
    protected float horizontalInput;
    protected float verticalInput;
    protected float moveAmount;
    protected Vector3 moveDirection;
    protected float currentSpeed;
    [HideInInspector] public PlayerMovementState currentMovementState;
    
    [Header("Rotation Settings")]
    protected Vector3 targetRotationDirection;
    protected Quaternion targetRotation;
    protected Quaternion finalRotation;
    
    Vector3 jumpDirection;
    
    protected override void Awake()
    {
        base.Awake();
        
        player = GetComponent<Player>();
        
        mainCamTransform = Camera.main.transform;
    }

    protected override void Update()
    {
        base.Update();

        HandleJumpMovement();
        HandleFreeFallMovement();
    }

    protected override void GetMovementInputs()
    {
        horizontalInput = player.inputManager.horizontalInput;
        verticalInput = player.inputManager.verticalInput;
    }
    
    protected override void HandleGroundedMovement()
    {
        horizontalInput = player.inputManager.horizontalInput;
        verticalInput = player.inputManager.verticalInput;
        
        camForward = mainCamTransform.forward;
        camForward.y = 0;
        camForward.Normalize();
        
        camRight = mainCamTransform.right;
        camRight.y = 0;
        camRight.Normalize();
        
        if (!player.canMove) return;
        
        moveDirection = camForward * verticalInput;
        moveDirection += camRight * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.isSprinting)
        {
            currentSpeed = player.sprintingSpeed;
            currentMovementState = PlayerMovementState.Sprinting;
        }
        else
        {
            if (moveAmount > 0.5f)
            {
                currentSpeed = player.runningSpeed;
                currentMovementState = PlayerMovementState.Running;
            }
            else if (moveAmount <= 0.5f)
            {
                currentSpeed = player.walkingSpeed;
                currentMovementState = PlayerMovementState.Walking;
            }
        }
        
        player.controller.Move(currentSpeed * Time.deltaTime * moveDirection);
    }

    protected override void HandleCharacterAnimation()
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

        if (moveAmount != 0)
        {
            player.playerAnimatorManager.IsMoving = true;
        }
        else
        {
            player.playerAnimatorManager.IsMoving = false;
        }

        player.playerAnimatorManager.SetAnimatorParameters(0, moveAmount);
    }

    protected override void HandleCharacterRotation()
    {
        if (!player.canRotate) return;
        
        targetRotationDirection = camForward * verticalInput;
        targetRotationDirection += camRight * horizontalInput;
        targetRotationDirection.y = 0f;
        targetRotationDirection.Normalize();

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = player.transform.forward;
        }

        targetRotation = Quaternion.LookRotation(targetRotationDirection);
        
        finalRotation = Quaternion.Slerp(player.transform.rotation, targetRotation, player.rotationDampTime * Time.deltaTime);
        player.transform.rotation = finalRotation;
    }

    public void HandleSprinting()
    {
        if (player.performingAction)
        {
            player.isSprinting = false;
        }

        player.isSprinting = moveAmount > 0.5f;
    }

    public void PerformDash()
    {
        if (player.performingAction) return;

        Vector3 dashDirection;

        if (moveAmount > 0)
        {
            dashDirection = camForward * verticalInput;
            dashDirection += camRight * horizontalInput;
            dashDirection.y = 0f;
            dashDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(dashDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimatorManager.PlayForwardDash();
        }
        else
        {
            player.playerAnimatorManager.PlayBackwardDash();
        }
    }

    public void PerformJump()
    {
        if (player.performingAction) return;

        if (player.isJumping) return;

        if (!player.playerAnimatorManager.IsGrounded) return;

        player.playerAnimatorManager.PlayJumpAction();
        player.isJumping = true;

        jumpDirection = camForward * verticalInput;
        jumpDirection += camRight * horizontalInput;
        jumpDirection.y = 0f;

        if (jumpDirection != Vector3.zero)
        {
            if (player.isSprinting)
            {
                jumpDirection *= 1;
            }
            else if (moveAmount > 0.5f)
            {
                jumpDirection *= 0.5f;
            }
            else if (moveAmount <= 0.5f)
            {
                jumpDirection *= 0.25f;
            }
        }
    }

    private void HandleJumpMovement()
    {
        if (player.isJumping)
        {
            player.controller.Move(jumpDirection * player.jumpForwardVelocity * Time.deltaTime);
        }
    }

    private void HandleFreeFallMovement()
    {
        if (!player.isGrounded)
        {
            Vector3 freeFallDirection = Vector3.zero;

            freeFallDirection = camForward * verticalInput;
            freeFallDirection += camRight * horizontalInput;
            freeFallDirection.y = 0f;

            player.controller.Move(freeFallDirection * player.freeFallControlVelocity * Time.deltaTime);
        }
    }

    public void ApplyJumpVelocity()
    {
        yVelocity.y = Mathf.Sqrt(player.jumpHeight * -2 * gravityForce);
        
        player.playerSoundFXManager.PlayJumpSound();
    }
}
