using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [HideInInspector] public NavMeshAgent navMeshAgent;

    [HideInInspector] public EnemyAnimatorManager enemyAnimatorManager;
    [HideInInspector] public EnemyMovementManager enemyMovementManager;

    protected override void Awake()
    {
        base.Awake();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyMovementManager = GetComponent<EnemyMovementManager>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        characterStateMachine.currentState.PhysicsUpdate();
    }

    protected override void Update()
    {
        base.Update();
        
        characterStateMachine.currentState.HandleInput();
        characterStateMachine.currentState.LogicUpdate();
    }

    protected override void InitializeStates()
    {
        base.InitializeStates();

        idleState = new IdleState(this, characterStateMachine);

        characterStateMachine.Initialize(idleState);
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        GUI.color = Color.red;
        GUI.Label(new Rect(0, 50, 200, 20), this.GetType().Name + " : " + characterStateMachine.currentState.ToString());
    }
}
