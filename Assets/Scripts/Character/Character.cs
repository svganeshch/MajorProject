using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Movement Controls")]
    public float moveSpeed = 5f;

    [Header("Character Animation Smoothing Controls")]
    public float speedDampTime = 0.1f;
    public float rotationDampTime = 15f;

    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public StateMachine characterStateMachine;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterMovementManager characterMovementManager;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        characterMovementManager = GetComponent<CharacterMovementManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();

        characterStateMachine = new StateMachine();

    }

    private void FixedUpdate()
    {
        characterStateMachine.currentState.PhysicsUpdate();
    }

    protected virtual void Update()
    {
        characterStateMachine.currentState.HandleInput();
        characterStateMachine.currentState.LogicUpdate();
    }


}
