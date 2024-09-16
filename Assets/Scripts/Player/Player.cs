using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [HideInInspector] public InputManager inputManager;

    protected override void Awake()
    {
        base.Awake();

        inputManager = GetComponent<InputManager>();
    }
}
