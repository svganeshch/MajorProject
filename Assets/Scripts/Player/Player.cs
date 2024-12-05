using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerMovementManager playerMovementManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public InputManager inputManager;

    [Header("Jump Controls")]
    public float jumpHeight = 5f;
    public float jumpForwardVelocity = 5f;
    public float freeFallControlVelocity = 2f;

    protected override void Awake()
    {
        base.Awake();

        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerMovementManager = GetComponent<PlayerMovementManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        inputManager = GetComponent<InputManager>();
    }

    protected override void InitializeStates()
    {
        base.InitializeStates();

        idleState = new IdleState(this, characterStateMachine);
        liteAttackState = new AttackState(this, characterStateMachine, true);
        heavyAttackState = new AttackState(this, characterStateMachine, false);

        characterStateMachine.Initialize(idleState);
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        GUI.color = Color.red;
        GUI.Label(new Rect(0, 0, 200, 20), this.GetType().Name + " : " + characterStateMachine.currentState.ToString());
    }
}
