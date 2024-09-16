using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterAnimatorManager : MonoBehaviour
{
    Character character;

    private static readonly int isGroundedHash = Animator.StringToHash("isGrounded");

    public bool IsGrounded
    {
        get => character.animator.GetBool(isGroundedHash);
        set => character.animator.SetBool(isGroundedHash, value);
    }

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void SetAnimatorParameters(float horizontalInput, float verticalInput)
    {
        character.animator.SetFloat("speedX", horizontalInput, character.speedDampTime, Time.deltaTime);
        character.animator.SetFloat("speedY", verticalInput, character.speedDampTime, Time.deltaTime);
    }
}
