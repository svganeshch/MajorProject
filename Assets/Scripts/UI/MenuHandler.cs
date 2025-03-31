using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public static MenuHandler Instance;
    
    public GameObject mainMenuPanel;
    public GameObject playerHudPanel;
    public GameObject deathMenuPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void OnStartButton()
    {
        mainMenuPanel.SetActive(false);
        playerHudPanel.SetActive(true);
        
        Time.timeScale = 1;
    }

    public void OnRetryButton()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        
        deathMenuPanel.SetActive(false);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
