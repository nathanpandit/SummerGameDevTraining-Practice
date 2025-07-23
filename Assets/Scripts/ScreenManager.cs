using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>
{
    public ScreenType currentScreenType = ScreenType.StartScreen;
    public GameObject startScreen, winScreen, loseScreen, gameScreen, pauseScreen, exitScreen;
    public GameObject currentScreen;
    
    void Start()
    {
        
    }

    void Update()
    {
        switch (currentScreenType)
        {
            case ScreenType.StartScreen:
                break;
            
            case ScreenType.WinScreen:
                break;
            case ScreenType.LoseScreen:
                break;
            case ScreenType.GameScreen:
                break;
            case ScreenType.PauseScreen:
                break;
            case ScreenType.ExitScreen:
                break;
            default:
                break;
        }
    }

    public void SetCurrentScreen(ScreenType screenType)
    {
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