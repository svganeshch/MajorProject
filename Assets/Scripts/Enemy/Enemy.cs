using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [HideInInspector] public NavMeshAgent navMeshAgent;

    [HideInInspector] public EnemyAnimatorManager enemyAnimatorManager;
    [HideInInspector] public EnemyCombatManager enemyCombatManager;
    [HideInInspector] public EnemyInventoryManager enemyInventoryManager;
    [HideInInspector] public EnemyMovementManager enemyMovementManager;
    
    // Enemy AI States
    private StateMachine enemyStateMachine;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public PursueState pursueState;
    [HideInInspector] public CombatState combatState;
    [HideInInspector] public AttackState attackState;

    protected override void Awake()
    {
        base.Awake();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyCombatManager = GetComponent<EnemyCombatManager>();
        enemyInventoryManager = GetComponent<EnemyInventoryManager>();
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
        
        HandleNavMeshAgent();
    }

    private void HandleNavMeshAgent()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
        
        if (navMeshAgent.enabled)
        {
            Vector3 agentDestination = navMeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainingDistance > navMeshAgent.stoppingDistance)
            {
                enemyAnimatorManager.IsMoving = true;
            }
            else
            {
                enemyAnimatorManager.IsMoving = false;
            }
        }
        else
        {
            enemyAnimatorManager.IsMoving = false;
        }
    }

    public void SetNavMeshAgentDestination()
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(characterCombatManager.currentTarget.transform.position, path);
        navMeshAgent.SetPath(path);
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
        combatState = new CombatState(this, enemyStateMachine);
        attackState = new AttackState(this, enemyStateMachine);

        // Initial State
        enemyStateMachine.Initialize(idleState);
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        //GUI.color = Color.red;
        //GUI.Label(new Rect(0, 50, 200, 20), this.GetType().Name + " : " + enemyStateMachine.currentState.ToString());
    }
}
