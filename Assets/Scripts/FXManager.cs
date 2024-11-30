using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;

    public List<ParticleSystem> currentParticleEffects = new List<ParticleSystem>();
    public List<VisualEffect> currentVisualEffects = new List<VisualEffect>();

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

    public void PlayEffect(string effect)
    {
        switch (effect)
        {
            case "sword":
                weaponSlashEffect.Play();
                currentParticleEffects.Add(weaponSlashEffect);
                break;

            case "dash":
                forwardDashEffect.Play();
                currentVisualEffects.Add(forwardDashEffect);
                break;

            default:
                Debug.Log("PLAY: UNKNOWN EFFECT!!");
                break;

        }
    }

    public void StopEffect(string effect)
    {
        switch (effect)
        {
            case "sword":
                weaponSlashEffect.Stop();
                currentParticleEffects.Remove(weaponSlashEffect);
                break;

            case "dash":
                forwardDashEffect.Stop();
                currentVisualEffects.Remove(forwardDashEffect);
                break;

            default:
                Debug.Log("STOP: UNKNOWN EFFECT!!");
                break;

        }
    }

    public void StopAllEffects()
    {
        if (currentParticleEffects.Count <= 0 && currentVisualEffects.Count <= 0) return;

        foreach (var effect in currentParticleEffects)
        {
            effect.Stop();
        }

        foreach (var effect in currentVisualEffects)
        {
            effect.Stop();
        }

        currentParticleEffects.Clear();
        currentVisualEffects.Clear();
    }
}
