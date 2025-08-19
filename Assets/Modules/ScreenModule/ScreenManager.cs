using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScreenManager : MonoBehaviour
{
    public ScreenType currentScreenType;

    // public ExitScreen exitScreen;
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
    }

    void Start()
    {
        HideAllScreens();
        ShowScreen(ScreenType.StartScreen);
    }

    public void ShowScreen(ScreenType screenType)
    {
        var currentScreen = screens.FirstOrDefault(s => s.type == screenType)?.gameObject;
        currentScreen.SetActive(true);
    }

    public void HideScreen(ScreenType screenType)
    {
        var screen = screens.FirstOrDefault(s => s.type == screenType)?.gameObject;
        if (screen != null)
        {
            screen.SetActive(false);
        }
    }

    public void HideAllBaseScreens()
    {
        foreach (var screen in screens)
        {
            Debug.Log("Setting screen " + screen.type + " to inactive");
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
    }
    
    
}