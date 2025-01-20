using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUDManager : MonoBehaviour
{
    public static PlayerHUDManager instance;
    
    private UIDocument document;
    
    private ProgressBar healthBar;
    private const string healthProgressBarName = "HealthBar";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        document = GetComponent<UIDocument>();
        
        healthBar = document.rootVisualElement.Q<ProgressBar>(healthProgressBarName);
    }

    public void SetHealthBar(float value)
    {
        healthBar.value = value;
    }
}