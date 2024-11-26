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

    private static readonly int liteAttack1Hash = Animator.StringToHash("lite_attack1");
    private static readonly int liteAttack2Hash = Animator.StringToHash("lite_attack2");
    private static readonly int liteAttack3Hash = Animator.StringToHash("lite_attack3");

    private static readonly int heavyAttack1Hash = Animator.StringToHash("heavy_attack1");
    private static readonly int heavyAttack2Hash = Animator.StringToHash("heavy_attack2");

    private static readonly int forwardDash = Animator.StringToHash("Forward_Dash");
    private static readonly int backwardDash = Animator.StringToHash("Backward_Dash");

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

    public void PlayLiteAttackAction(bool canCombo, bool canRotate = false)
    {
        int nextAttackHash = liteAttack1Hash;

        if (canCombo)
        {
            if (previousActionHash == liteAttack1Hash)
                nextAttackHash = liteAttack2Hash;
            else if (previousActionHash == liteAttack2Hash)
                nextAttackHash = liteAttack1Hash;
        }

        PlayCharacterActionAnimation(nextAttackHash, true, canRotate);
    }

    public void PlayHeavyAttackAction(bool canCombo, bool canRotate = false)
    {
        int nextAttackHash = heavyAttack1Hash;

        if (canCombo)
        {
            if (previousActionHash == heavyAttack1Hash)
                nextAttackHash = heavyAttack2Hash;
            else if (previousActionHash == heavyAttack1Hash)
                nextAttackHash = heavyAttack2Hash;
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
}
