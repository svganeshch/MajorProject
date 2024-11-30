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

        character.isJumping = false;
    }
}
