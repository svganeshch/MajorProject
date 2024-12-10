using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    Player player;

    public Vector2 moveInput;
    public float horizontalInput;
    public float verticalInput;

    [HideInInspector] PlayerInput playerInput;

    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction liteAttackAction;
    [HideInInspector] public InputAction jumpAction;
    [HideInInspector] public InputAction forwardDashAction;
    [HideInInspector] public InputAction backDashAction;

    [HideInInspector] public bool liteAttackInput = false;
    [HideInInspector] public bool jumpInput = false;
    [HideInInspector] public bool forwardDashInput = false;
    [HideInInspector] public bool backDashInput = false;

    bool input_que_active = false;
    float default_que_input_timer = 0.35f;
    float que_input_timer;

    bool attack_que = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        moveAction = playerInput.actions["Move"];
        moveAction.performed += i => moveInput = i.ReadValue<Vector2>();

        liteAttackAction = playerInput.actions["LiteAttack"];
        liteAttackAction.performed += i => liteAttackInput = true;

        jumpAction = playerInput.actions["Jump"];
        jumpAction.performed += i => jumpInput = true;

        forwardDashAction = playerInput.actions["ForwardDash"];
        forwardDashAction.performed += i => forwardDashInput = true;

        backDashAction = playerInput.actions["BackDash"];
        backDashAction.performed += i => backDashInput = true;
    }

    private void Update()
    {
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;

        HandleInputActions();
        HandleQuedInputs();
    }

    private void HandleInputActions()
    {
        HandleAttackInput();
        HandleDashInput();
        HandleJumpInput();
    }

    private void HandleAttackInput()
    {
        if (liteAttackInput)
        {
            liteAttackInput = false;

            player.playerCombatManager.PerformWeaponBasedAction(
                player.playerInventoryManager.currentRightHandWeapon.liteAttackAction,
                player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleDashInput()
    {
        if (forwardDashInput || backDashInput)
        {
            forwardDashInput = false;
            backDashInput = false;

            player.playerMovementManager.PerformDash();
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            
            if (player.playerParkourManager.IsParkourObstacle())
            {
                player.playerParkourManager.PerformParkourActionCoroutine();
                return;
            }

            player.playerMovementManager.PerformJump();
        }
    }

    private void QueInput(ref bool quedInput)
    {
        ResetQueFlags();

        if (player.performingAction)
        {
            quedInput = true;
            que_input_timer = default_que_input_timer;
            input_que_active = true;
        }
    }

    private void ProcessQuedInputs()
    {
        if (attack_que) liteAttackInput = true;
    }

    private void HandleQuedInputs()
    {
        if (input_que_active)
        {
            if (que_input_timer > 0)
            {
                que_input_timer -= Time.deltaTime;
                ProcessQuedInputs();
            }
            else
            {
                ResetQueFlags();
            }
        }
    }

    private void ResetQueFlags()
    {
        attack_que = false;

        input_que_active = false;
        que_input_timer = 0;
    }
}
