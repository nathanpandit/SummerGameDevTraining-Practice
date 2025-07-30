using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScreenManager : MonoBehaviour
{
    public ScreenType currentScreenType;
    public StartScreen startScreen;
    public WinScreen winScreen;
    public LoseScreen loseScreen;
    public PauseScreen pauseScreen;
    // public ExitScreen exitScreen;
    public GameObject currentScreen;
    private static ScreenManager _instance;

    public static ScreenManager Instance()
    {
        if (_instance == null)
        {
            _instance = FindFirstObjectByType<ScreenManager>();
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(ScreenManager).Name);
                _instance = obj.AddComponent<ScreenManager>();
            }
        }
        return _instance;
    }

    void Awake()
    {
        ShowScreen(ScreenType.StartScreen);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowScreen(ScreenType.PauseScreen);
        }
    }

    public void ShowScreen(ScreenType screenType)
    {
        if(currentScreen != null)
        {
            currentScreen.SetActive(false);
        }
        currentScreenType = screenType;
        switch (currentScreenType)
        {
            case ScreenType.StartScreen:
                currentScreen = startScreen.gameObject;
                currentScreen.SetActive(true);
                break;
            
            case ScreenType.WinScreen:
                currentScreen = winScreen.gameObject;
                currentScreen.SetActive(true);
                break;
            
            case ScreenType.LoseScreen:
                currentScreen = loseScreen.gameObject;
                currentScreen.SetActive(true);
                break;
            
            case ScreenType.GameScreen:
                currentScreen.SetActive(false);
                break;
            
            case ScreenType.PauseScreen:
                currentScreen = pauseScreen.gameObject;
                currentScreen.SetActive(true);
                break;
            
            /*case ScreenType.ExitScreen:
                currentScreen = exitScreen;
                currentScreen.SetActive(true);
                break;
                */
            
            default:
                currentScreen.SetActive(false);
                break;
        }

    }
    
    public void HideCurrentScreen()
    {
        if (currentScreen != null)
        {
            currentScreen.SetActive(false);
        }
    }

    public void SetCurrentScreen(ScreenType screenType)
    {
        currentScreenType = screenType;
    }
}