using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class State
{
    protected Character character;
    protected StateMachine stateMachine;

    public State(Character _character, StateMachine _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public virtual void Enter()
	{
		// Debug.Log("Entered state " + character.name + " : " + this);
	}

	public virtual void HandleInput() { }

	public virtual void LogicUpdate() { }

	public virtual void PhysicsUpdate() { }

	public virtual void Exit()
	{
		// Debug.Log("Exited state " + character.name + " : " + this);
	}
}
