using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Vector2 moveInput;
    public float horizontalInput;
    public float verticalInput;

    [HideInInspector] PlayerInput playerInput;

    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction liteAttackAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        moveAction = playerInput.actions["Move"];
        moveAction.performed += i => moveInput = i.ReadValue<Vector2>();

        liteAttackAction = playerInput.actions["LiteAttack"];
    }

    private void Update()
    {
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;
    }
}
