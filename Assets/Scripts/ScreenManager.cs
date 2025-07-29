using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>
{
    public ScreenType currentScreenType = ScreenType.StartScreen;
    public GameObject startScreen, winScreen, loseScreen, gameScreen, pauseScreen, exitScreen;
    public GameObject currentScreen;
    
    void Start()
    {
        SetCurrentScreen(ScreenType.StartScreen);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCurrentScreen(ScreenType.PauseScreen);
        }
    }

    public void SetCurrentScreen(ScreenType screenType)
    {
        if(currentScreen != null)
        {
            currentScreen.SetActive(false);
        }
        currentScreenType = screenType;
        switch (currentScreenType)
        {
            case ScreenType.StartScreen:
                currentScreen = startScreen;
                currentScreen.SetActive(true);
                break;
            
            case ScreenType.WinScreen:
                currentScreen = winScreen;
                currentScreen.SetActive(true);
                break;
            
            case ScreenType.LoseScreen:
                currentScreen = loseScreen;
                currentScreen.SetActive(true);
                break;
            
            case ScreenType.GameScreen:
                currentScreen.SetActive(false);
                break;
            
            case ScreenType.PauseScreen:
                currentScreen = pauseScreen;
                currentScreen.SetActive(true);
                break;
            
            case ScreenType.ExitScreen:
                currentScreen = exitScreen;
                currentScreen.SetActive(true);
                break;
            
            default:
                currentScreen.SetActive(false);
                break;
        }

    }
    
    public void DeactivateCurrentScreen()
    {
        if (currentScreen != null)
        {
            currentScreen.SetActive(false);
        }
    }
}

public enum ScreenType
{
    StartScreen,
    WinScreen,
    LoseScreen,
    GameScreen,
    PauseScreen,
    ExitScreen
}