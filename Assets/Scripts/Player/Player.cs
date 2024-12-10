using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;
    [HideInInspector] public PlayerMovementManager playerMovementManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerParkourManager playerParkourManager;
    [HideInInspector] public InputManager inputManager;

    [Header("Jump Controls")]
    public float jumpHeight = 5f;
    public float jumpForwardVelocity = 5f;
    public float freeFallControlVelocity = 2f;

    protected override void Awake()
    {
        base.Awake();

        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerMovementManager = GetComponent<PlayerMovementManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerParkourManager = GetComponent<PlayerParkourManager>();
        inputManager = GetComponent<InputManager>();
    }
}
