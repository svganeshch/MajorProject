using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    Player player;
    Transform playerTransform;

    private static readonly int jumpHash = Animator.StringToHash("jump");
    
    private static readonly int climbHash = Animator.StringToHash("Climb");
    private static readonly int vaultHash = Animator.StringToHash("Vault");

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
        playerTransform = player.transform;
    }

    private void OnAnimatorMove()
    {
        if (player.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;

            player.controller.Move(velocity);
            playerTransform.rotation *= player.animator.deltaRotation;
        }
    }

    public void PlayJumpAction()
    {
        PlayCharacterActionAnimation(jumpHash, true, false, false, false);
    }
    
    public void PlayParkourAction(ParkourActionType parkourAction)
    {
        int parkourActionHash;
        
        switch (parkourAction)
        {
            case ParkourActionType.Climb:
                parkourActionHash = climbHash;
                player.playerSoundFXManager.PlayClimbSound();
                break;
            
            case ParkourActionType.Vault:
                parkourActionHash = vaultHash;
                player.playerSoundFXManager.PlayVaultSound();
                break;
            
            default:
                parkourActionHash = 0;
                break;
        }

        PlayCharacterActionAnimation(parkourActionHash, true);
    }
}
