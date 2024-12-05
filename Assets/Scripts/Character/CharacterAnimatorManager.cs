using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterAnimatorManager : MonoBehaviour
{
    Character character;

    private int previousActionHash;

    private static readonly int isGroundedHash = Animator.StringToHash("isGrounded");
    private static readonly int inAirTimeHash = Animator.StringToHash("inAirTime");

    private int[] LiteAttack =
    {
        Animator.StringToHash("lite_attack1"),
        Animator.StringToHash("lite_attack2"),
        Animator.StringToHash("lite_attack3")
    };

    private int[] HeavyAttack =
    {
        Animator.StringToHash("heavy_attack1"),
        Animator.StringToHash("heavy_attack2")
    };

    private static readonly int forwardDash = Animator.StringToHash("Forward_Dash");
    private static readonly int backwardDash = Animator.StringToHash("Backward_Dash");

    private static readonly int deathHash = Animator.StringToHash("Death");

    public bool IsGrounded
    {
        get => character.animator.GetBool(isGroundedHash);
        set => character.animator.SetBool(isGroundedHash, value);
    }

    public float InAirTime
    {
        get => character.animator.GetFloat(inAirTimeHash);
        set => character.animator.SetFloat(inAirTimeHash, value);
    }

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }

    public void SetAnimatorParameters(float horizontalInput, float verticalInput)
    {
        character.animator.SetFloat("speedX", horizontalInput, character.speedDampTime, Time.deltaTime);
        character.animator.SetFloat("speedY", verticalInput, character.speedDampTime, Time.deltaTime);
    }

    protected virtual void PlayCharacterActionAnimation(
        int animationClipHash,
        bool isPerformingAction = true,
        bool canRotate = false,
        bool canMove = false,
        bool applyRootMotion = true)
    {
        previousActionHash = animationClipHash;

        character.animator.CrossFade(animationClipHash, character.animationFadeTime);

        character.performingAction = isPerformingAction;
        character.applyRootMotion = applyRootMotion;
        character.canRotate = canRotate;
        character.canMove = canMove;
    }

    public void PlayAttackAction(State attackState, bool canCombo, bool canRotate = false)
    {
        int[] attackHashes = new int[LiteAttack.Length];

        if (attackState == character.liteAttackState)
        {
            attackHashes = LiteAttack;
        }
        else if (attackState == character.heavyAttackState)
        {
            attackHashes = HeavyAttack;
        }

        int nextAttackHash = attackHashes[0];

        if (canCombo)
        {
            if (previousActionHash == attackHashes[0])
                nextAttackHash = attackHashes[1];
            else if (previousActionHash == attackHashes[1])
                nextAttackHash = attackHashes[0];
        }

        PlayCharacterActionAnimation(nextAttackHash, true, canRotate);
    }

    public void PlayForwardDash()
    {
        PlayCharacterActionAnimation(forwardDash, true);
    }

    public void PlayBackwardDash()
    {
        PlayCharacterActionAnimation(backwardDash, true);
    }

    public void PlayDeathAction()
    {
        PlayCharacterActionAnimation(deathHash, true);
    }
}
