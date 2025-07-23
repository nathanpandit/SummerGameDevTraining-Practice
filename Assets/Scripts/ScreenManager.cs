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
        switch (currentScreenType)
        {
            case ScreenType.StartScreen:
                currentScreen = startScreen;
                break;
            
            case ScreenType.WinScreen:
                currentScreen = winScreen;
                break;
            
            case ScreenType.LoseScreen:
                currentScreen = loseScreen;
                break;
            
            case ScreenType.GameScreen:
                currentScreen = null;
                break;
            
            case ScreenType.PauseScreen:
                currentScreen = pauseScreen;
                break;
            
            case ScreenType.ExitScreen:
                currentScreen = exitScreen;
                break;
            
            default:
                currentScreen.SetActive(false);
                break;
        }
    }

    public void SetCurrentScreen(ScreenType screenType)
    {
        if(currentScreen != null)
        {
            currentScreen.SetActive(false);
        }
        currentScreenType = screenType;
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