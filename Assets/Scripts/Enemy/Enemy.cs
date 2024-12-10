using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [HideInInspector] public NavMeshAgent navMeshAgent;

    [HideInInspector] public EnemyAnimatorManager enemyAnimatorManager;
    [HideInInspector] public EnemyCombatManager enemyCombatManager;
    [HideInInspector] public EnemyMovementManager enemyMovementManager;
    
    // Enemy AI States
    private StateMachine enemyStateMachine;
    [HideInInspector] public State idleState;
    [HideInInspector] public State pursueState;

    protected override void Awake()
    {
        base.Awake();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyCombatManager = GetComponent<EnemyCombatManager>();
        enemyMovementManager = GetComponent<EnemyMovementManager>();
    }

    protected override void Start()
    {
        base.Start();
        
        InitializeStates();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        enemyStateMachine.currentState.PhysicsUpdate();
    }

    protected override void Update()
    {
        base.Update();
        
        enemyStateMachine.currentState.HandleInput();
        enemyStateMachine.currentState.LogicUpdate();
    }

    private void InitializeStates()
    {
        // Statemachine
        enemyStateMachine = new StateMachine();
        
        // States
        idleState = new IdleState(this, enemyStateMachine);
        pursueState = new PursueState(this, enemyStateMachine);

        // Initial State
        enemyStateMachine.Initialize(idleState);
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        GUI.color = Color.red;
        GUI.Label(new Rect(0, 50, 200, 20), this.GetType().Name + " : " + enemyStateMachine.currentState.ToString());
    }
}
