using UnityEngine;

public class ResetJumpState : StateMachineBehaviour
{
    Character character;
    Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null)
        {
            character = animator.GetComponent<Character>();
            
            if (character is Player)
                player = animator.GetComponent<Player>();
        }

        character.isJumping = false;

        if (player != null)
        {
            player.playerSoundFXManager.PlayLandingSound();
        }
    }
}
