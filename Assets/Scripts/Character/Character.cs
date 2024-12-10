using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    [Header("Character Stats")]
    public int health = 100;

    [Header("Character Movement Controls")]
    public float walkingSpeed = 5f;
    public float runningSpeed = 7.5f;
    public float sprintingSpeed = 10f;

    [Header("Character Smoothing Controls")]
    public float animationFadeTime = 0.2f;
    public float speedDampTime = 0.1f;
    public float rotationDampTime = 15f;

    [Header("Flags")]
    public bool isDead = false;
    public bool performingAction = false;
    public bool performingParkour = false;
    public bool canCombo = false;
    public bool canMove = true;
    public bool canRotate = true;
    public bool applyRootMotion = false;
    public bool isJumping = false;
    public bool isGrounded = true;
    public bool isSprinting = false;

    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;
    [HideInInspector] public CharacterMovementManager characterMovementManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterHealthManager characterHealthManager;
    [HideInInspector] public CharacterInventoryManager CharacterInventoryManager;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        characterMovementManager = GetComponent<CharacterMovementManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterHealthManager = GetComponent<CharacterHealthManager>();
        CharacterInventoryManager = GetComponent<CharacterInventoryManager>();
    }

    protected virtual void Start()
    {
        IgnoreOwnColliders();
    }

    protected virtual void FixedUpdate()
    {
        
    }

    protected virtual void Update()
    {
        
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
