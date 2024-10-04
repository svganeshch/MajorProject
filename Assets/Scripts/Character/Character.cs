using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Movement Controls")]
    public float moveSpeed = 5f;
    public float jumpHeight = 5f;
    public float jumpForwardVelocity = 5f;
    public float freeFallControlVelocity = 2f;

    [Header("Character Animation Smoothing Controls")]
    public float animationFadeTime = 0.2f;
    public float speedDampTime = 0.1f;
    public float rotationDampTime = 15f;

    [Header("Flags")]
    public bool performingAction = false;
    public bool canCombo = false;
    public bool canMove = true;
    public bool canRotate = true;
    public bool applyRootMotion = false;

    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public StateMachine characterStateMachine;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterMovementManager characterMovementManager;

    // Character States
    [HideInInspector] public State idleState;
    [HideInInspector] public State liteAttackState;
    [HideInInspector] public State heavyAttackState;
    [HideInInspector] public State jumpState;

    protected virtual void InitializeStates() { }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        characterMovementManager = GetComponent<CharacterMovementManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();

        characterStateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        InitializeStates();
    }

    protected virtual void FixedUpdate()
    {
        characterStateMachine.currentState.PhysicsUpdate();
    }

    protected virtual void Update()
    {
        characterStateMachine.currentState.HandleInput();
        characterStateMachine.currentState.LogicUpdate();
    }

    protected virtual void OnGUI() { }
}
