using UnityEngine;

public class PlayerMovementManager : CharacterMovementManager
{
    Player player;
    
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

    protected override void GetMovementInputs()
    {
        horizontalInput = player.inputManager.horizontalInput;
        verticalInput = player.inputManager.verticalInput;
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
