using UnityEngine;

public class ResetJumpState : StateMachineBehaviour
{
    Character character;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null)
        {
            character = animator.GetComponent<Character>();
        }

        if (character.characterStateMachine.currentState != null)
        {
            if (character.characterStateMachine.currentState == character.jumpState)
            {
                character.characterStateMachine.ChangeState(character.idleState);
            }
        }
    }
}
