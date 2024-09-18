using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
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
