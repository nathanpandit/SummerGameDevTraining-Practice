using System.Linq;
using TMPro;
using UfoPuzzle;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : BaseScreen
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button getRewardButton;
    public TextMeshProUGUI coinRewardText;
    public TextMeshProUGUI totalCoinCountText;
    void Awake()
    {
        type = ScreenType.WinScreen;
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
        totalCoinCountText.text = InventoryHelper.Instance().GetQuantity(InventoryType.Coin).ToString();
        ShowCountOnLevelWon(InventoryType.Coin);
        InventoryHelper.Instance().AddItem(InventoryType.Coin, GameManager.coinCount);
    }
    
    public void ShowCountOnLevelWon(InventoryType itemType)
    {
            int coinsWon = GameManager.coinCount;
            coinRewardText.text = (InventoryHelper.Instance().GetQuantity(itemType)-GameManager.coinCount).ToString();
            coinRewardText.gameObject.SetActive(true);
        }

    void OnGetRewardButtonClicked()
    {
        getRewardButton.gameObject.SetActive(false);
        CurveController.Instance().GrantReward(onGameWinAction);
    }

    void onGameWinAction()
    {
        EventManager.Instance().OnGameStart();
    }
    
    
}

public class WinScreenParameter : BaseScreenParameter
{
    public int coinCount;
}