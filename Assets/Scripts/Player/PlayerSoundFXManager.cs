using UnityEngine;

public class PlayerSoundFXManager : CharacterSoundFXManager
{
    [Header("Jump Sounds")]
    public AudioClip[] jumpSounds;
    
    [Header("Landing Sounds")]
    public AudioClip[] landingSounds;
    
    [Header("Parkour Sounds")]
    public AudioClip[] vaultSounds;
    public AudioClip[] climbSounds;

    public void PlayJumpSound()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(jumpSounds));
    }

    public void PlayLandingSound()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(landingSounds));
    }

    public void PlayVaultSound()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(vaultSounds));
    }

    public void PlayClimbSound()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(climbSounds));
    }
}