using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;
    
    private AudioSource audioSource;
    
    public AudioClip[] physicalDamageSFX;
    
    public AudioClip axeSwingSFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySoundFX(AudioClip soundFX, AudioSource ownAudioSource = null, float volume = 1, bool randomizePitch = false, float pitchRandom = 0.1f)
    {
        if (ownAudioSource != null) audioSource = ownAudioSource;
        
        audioSource.PlayOneShot(soundFX, volume);
        
        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning("ChooseRandomSFXFromArray: Clips array is null or empty.");
            return null;
        }
        
        int randomIndex = Random.Range(0, clips.Length);
        
        return clips[randomIndex];
    }
}
