using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : BaseScreen
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button nextLevelButton;
    public Button getRewardButton;
    public TextMeshProUGUI coinRewardText;
    public TextMeshProUGUI totalCoinCountText;
    void Awake()
    {
        type = ScreenType.WinScreen;
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        getRewardButton.onClick.AddListener(OnGetRewardButtonClicked);
    }

    public override void Initialize(BaseScreenParameter parameter = null)
    {
        var WinScreenParameters = parameter as WinScreenParameter;
        var coinCount = WinScreenParameters.coinCount;
    }

    public void OnEnable()
    {
        getRewardButton.gameObject.SetActive(true);
        totalCoinCountText.text = InventoryHelper.Instance().GetQuantityOnStart(InventoryType.Coin).ToString();
    }

    void OnNextLevelButtonClicked()
    {
        ScreenManager.Instance().HideScreen(ScreenType.WinScreen);
    }

    void OnGetRewardButtonClicked()
    {
        getRewardButton.gameObject.SetActive(false);
        CurveController.Instance().GrantReward();
    }
}

public class WinScreenParameter : BaseScreenParameter
{
    public int coinCount;
}