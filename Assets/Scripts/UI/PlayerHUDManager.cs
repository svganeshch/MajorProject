using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    public static PlayerHUDManager instance;
    
    public Image healthBarFill;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetHealthBar(float value)
    {
        healthBarFill.fillAmount = value;
    }
}