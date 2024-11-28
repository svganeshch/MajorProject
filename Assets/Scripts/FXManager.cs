using UnityEngine;
using UnityEngine.VFX;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;

    public ParticleSystem weaponSlashEffect;

    public VisualEffect forwardDashEffect;

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
