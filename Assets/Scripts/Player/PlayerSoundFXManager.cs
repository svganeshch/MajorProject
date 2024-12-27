using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSoundFXManager : CharacterSoundFXManager
{
    private Player player;
    
    [Header("Jump Sounds")]
    public AudioClip[] jumpEffortSounds;
    
    [Header("Landing Sounds")]
    public AudioClip[] landingEffortSounds;
    
    [Header("Parkour Sounds")]
    public AudioClip[] vaultSounds;
    public AudioClip[] climbSounds;
    
    [Header("Footsteps Sound Data")]
    public FootStepsData[] surfaceFootStepsData;

    protected override void Awake()
    {
        base.Awake();
        
        player = GetComponent<Player>();
    }

    public void PlayJumpSound()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(GetCurrentSurfaceFootStepData().jumpSounds));
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(jumpEffortSounds));
    }

    public void PlayLandingSound()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(GetCurrentSurfaceFootStepData().landingSounds));
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(landingEffortSounds));
    }

    public void PlayVaultSound()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(vaultSounds));
    }

    public void PlayClimbSound()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(climbSounds));
    }

    public override void PlayFootStepSound()
    {
        if (player.performingAction) return;
        
        FootStepsData currentFootStepsData = GetCurrentSurfaceFootStepData();

        if (currentFootStepsData != null)
        {
            AudioClip[] footStepSoundClips;
            
            switch (player.playerMovementManager.currentMovementState)
            {
                case PlayerMovementState.Walking:
                    footStepSoundClips = currentFootStepsData.walkingFootstepSounds;
                    break;
                
                case PlayerMovementState.Running:
                case PlayerMovementState.Sprinting:
                    footStepSoundClips = currentFootStepsData.runningFootstepSounds;
                    break;

                default:
                    footStepSoundClips = currentFootStepsData.walkingFootstepSounds;
                    break;
            }
            
            AudioClip footStepSound =
                WorldSoundFXManager.instance.ChooseRandomSFXFromArray(footStepSoundClips);
            
            PlaySoundFX(footStepSound, Random.Range(0.1f, 0.2f));
        }
    }

    private FootStepsData GetCurrentSurfaceFootStepData()
    {
        return FootStepsHandler.instance.CheckSurface(surfaceFootStepsData);
    }
}