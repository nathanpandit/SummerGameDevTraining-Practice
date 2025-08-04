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
    private RootScreen[] roots;

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
        roots = GetComponentsInChildren<RootScreen>(includeInactive: true);
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

    public void HideAllBaseScreens()
    {
        foreach (var screen in screens)
        {
            screen.gameObject.SetActive(false);
        }
    }

    public void HideAllRootScreens()
    {
        foreach(var root in roots)
        {
            root.gameObject.SetActive(false);
        }
    }
    
    public void HideAllScreens()
    {
        HideAllBaseScreens();
        HideAllRootScreens();
        currentScreen = null;
    }
    
    
}