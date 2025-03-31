using System;
using UnityEngine;

public class PowerUpHandler : MonoBehaviour
{
    public static PowerUpHandler Instance;

    public GameObject powerUpEnabled;
    public GameObject powerUpDisabled;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SetPowerUpStatus(bool status)
    {
        if (status)
        {
            powerUpEnabled.SetActive(true);
            powerUpDisabled.SetActive(false);
        }
        else
        {
            powerUpEnabled.SetActive(false);
            powerUpDisabled.SetActive(true);
        }
    }
}
