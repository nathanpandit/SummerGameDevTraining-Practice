using System.Collections.Generic;
using System.Linq;
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
    private BaseScreen[] screens;

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
        screens = GetComponentsInChildren<BaseScreen>(includeInactive: true);
        Debug.Log(screens.Length);
        ShowScreen(ScreenType.StartScreen);
    }

    void Start()
    {
        HideAllScreens();
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
        if (currentScreen != null)
        {
            currentScreen.SetActive(false);
        }

        currentScreenType = screenType;
        if (screenType == ScreenType.GameScreen)
        {
            HideCurrentScreen();
            return;
        }

        currentScreen = screens.FirstOrDefault(s => s.type == currentScreenType)?.gameObject;
        currentScreen.SetActive(true);
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

    public void HideAllScreens()
    {
        foreach (var screen in screens)
        {
            screen.gameObject.SetActive(false);
        }
    }
}