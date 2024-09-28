using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public void EnableCombo()
    {
        player.canCombo = true;
    }

    public void DisableCombo()
    {
        player.canCombo = false;
    }
}
