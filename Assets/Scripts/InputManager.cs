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

    [HideInInspector] public bool liteAttackInput = false;

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
    }

    private void HandleAttackInput()
    {
        if (liteAttackInput)
        {
            liteAttackInput = false;

            player.characterStateMachine.ChangeState(player.liteAttackState);
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
