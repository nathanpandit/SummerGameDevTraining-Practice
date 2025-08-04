using UnityEngine;
using UnityEngine.UI;

public class WinScreen : BaseScreen
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button nextLevelButton;
    void Awake()
    {
        type = ScreenType.WinScreen;
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
    }

    public override void Initialize(BaseScreenParameter parameter = null)
    {
        var WinScreenParameters = parameter as WinScreenParameter;
        var coinCount = WinScreenParameters.coinCount;
    }

    void OnNextLevelButtonClicked()
    {
        ScreenManager.Instance().HideScreen(ScreenType.WinScreen);
    }
}

public class WinScreenParameter : BaseScreenParameter
{
    public int coinCount;
}