using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Stats")]
    public int health = 100;

    [Header("Character Movement Controls")]
    public float moveSpeed = 5f;

    [Header("Character Smoothing Controls")]
    public float animationFadeTime = 0.2f;
    public float speedDampTime = 0.1f;
    public float rotationDampTime = 15f;

    [Header("Flags")]
    public bool isDead = false;
    public bool performingAction = false;
    public bool canCombo = false;
    public bool canMove = true;
    public bool canRotate = true;
    public bool applyRootMotion = false;
    public bool isJumping = false;
    public bool isVaulting = false;
    public bool isGrounded = true;

    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public StateMachine characterStateMachine;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterMovementManager characterMovementManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterHealthManager characterHealthManager;

    // Character States
    [HideInInspector] public State idleState;
    [HideInInspector] public State liteAttackState;
    [HideInInspector] public State heavyAttackState;

    protected virtual void InitializeStates() { }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        characterMovementManager = GetComponent<CharacterMovementManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterHealthManager = GetComponent<CharacterHealthManager>();

        characterStateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        InitializeStates();
        IgnoreOwnColliders();
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

    private void IgnoreOwnColliders()
    {
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] damagableCharacterColliders = GetComponentsInChildren<Collider>();
        List<Collider> ignoreColliders = new List<Collider>();

        foreach (var collider in damagableCharacterColliders)
        {
            ignoreColliders.Add(collider);
        }
        ignoreColliders.Add(characterControllerCollider);

        foreach (var collider in ignoreColliders)
        {
            foreach (var otherCollider in ignoreColliders)
            {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }
    }

    protected virtual void OnGUI() { }
}
