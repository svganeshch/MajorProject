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
    InputAction moveAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        moveAction = playerInput.actions["Move"];
        moveAction.performed += i => moveInput = i.ReadValue<Vector2>();
    }

    private void Update()
    {
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;
    }
}
