using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ResetAnimationFlags : StateMachineBehaviour
{
    Character character;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null)
        {
            character = animator.GetComponent<Character>();
        }

        character.performingAction = false;
        character.applyRootMotion = false;
        character.canRotate = true;
        character.canMove = true;
    }
}
