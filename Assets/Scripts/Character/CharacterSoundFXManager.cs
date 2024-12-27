using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    AudioSource audioSource;
    
    [Header("Attack Grunts")]
    public AudioClip[] attackGrunts;
    
    [Header("Damage Grunts")]
    public AudioClip[] damageGrunts;
    
    [Header("Death Grunts")]
    public AudioClip[] deathGrunts;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        audioSource.PlayOneShot(soundFX, volume);
        
        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public void PlayAttackGrunt()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts), 0.25f);
    }

    public void PlayDamageGrunt()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts), 1.5f);
    }

    public void PlayDeathGrunt()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(deathGrunts), 1.5f);
    }
    
    public virtual void PlayFootStepSound() {}
}
