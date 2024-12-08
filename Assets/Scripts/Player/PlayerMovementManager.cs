using UnityEngine;

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

    Vector3 jumpDirection;
    
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    protected override void Update()
    {
        base.Update();

        HandleJumpMovement();
        HandleFreeFallMovement();
    }

    protected override void HandleGroundedMovement()
    {
        horizontalInput = player.inputManager.horizontalInput;
        verticalInput = player.inputManager.verticalInput;
        
        if (!player.canMove) return;
        
        moveDirection = Vector3.forward * verticalInput;
        moveDirection += Vector3.right * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        
        player.controller.Move(player.moveSpeed * Time.deltaTime * moveDirection);
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

        player.playerAnimatorManager.SetAnimatorParameters(0, moveAmount);
    }

    protected override void HandleCharacterRotation()
    {
        if (!player.canRotate) return;

        targetRotationDirection = Vector3.forward * verticalInput;
        targetRotationDirection += Vector3.right * horizontalInput;
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

    public void PerformDash()
    {
        if (player.performingAction) return;

        Vector3 dashDirection;

        if (moveAmount > 0)
        {
            dashDirection = Vector3.forward * verticalInput;
            dashDirection += Vector3.right * horizontalInput;
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

        jumpDirection = Vector3.forward * verticalInput;
        jumpDirection += Vector3.right * horizontalInput;
        jumpDirection.y = 0f;

        if (moveAmount > 0.5f)
        {
            jumpDirection *= 0.5f;
        }
        else if (moveAmount <= 0.5f)
        {
            jumpDirection *= 0.25f;
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

            freeFallDirection = Vector3.forward * verticalInput;
            freeFallDirection += Vector3.right * horizontalInput;
            freeFallDirection.y = 0f;

            player.controller.Move(freeFallDirection * player.freeFallControlVelocity * Time.deltaTime);
        }
    }

    public void ApplyJumpVelocity()
    {
        yVelocity.y = Mathf.Sqrt(player.jumpHeight * -2 * gravityForce);
    }
}
