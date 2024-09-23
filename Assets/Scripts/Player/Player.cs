using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public InputManager inputManager;

    protected override void Awake()
    {
        base.Awake();

        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        inputManager = GetComponent<InputManager>();
    }

    protected override void InitializeStates()
    {
        base.InitializeStates();

        liteAttackState = new AttackState(this, characterStateMachine, true);
        heavyAttackState = new AttackState(this, characterStateMachine, false);
    }
}
