using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;

    CharacterController controller;
    InputManager inputManager;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        controller.Move(moveSpeed * Time.deltaTime * new Vector3(inputManager.horizontalInput, 0, inputManager.verticalInput));
    }
}
