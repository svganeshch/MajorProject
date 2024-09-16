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
    [HideInInspector] public CharacterMovementManager characterMovementManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        characterMovementManager = GetComponent<CharacterMovementManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
    }

    protected virtual void Update()
    {
        
    }
}
