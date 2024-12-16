using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    [HideInInspector] public PlayerChainAttackController playerChainAttackController;

    protected override void Awake()
    {
        base.Awake();

        playerChainAttackController = GetComponent<PlayerChainAttackController>();
    }
}