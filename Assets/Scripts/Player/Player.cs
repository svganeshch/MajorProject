using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerMovementManager playerMovementManager;
    [HideInInspector] public InputManager inputManager;

    protected override void Awake()
    {
        base.Awake();

        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerMovementManager = GetComponent<PlayerMovementManager>();
        inputManager = GetComponent<InputManager>();
    }

    protected override void InitializeStates()
    {
        base.InitializeStates();

        idleState = new IdleState(this, characterStateMachine);
        liteAttackState = new AttackState(this, characterStateMachine, true);
        heavyAttackState = new AttackState(this, characterStateMachine, false);
        jumpState = new JumpState(this, characterStateMachine);

        characterStateMachine.Initialize(idleState);
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        GUI.color = Color.red;
        GUI.Label(new Rect(0, 0, 200, 20), this.GetType().Name + " : " + characterStateMachine.currentState.ToString());
    }
}
