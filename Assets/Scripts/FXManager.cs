using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;

    public ParticleSystem weaponSlashEffect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
