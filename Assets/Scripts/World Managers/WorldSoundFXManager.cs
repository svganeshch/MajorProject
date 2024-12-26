using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    public AudioClip[] physicalDamageSFX;

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
