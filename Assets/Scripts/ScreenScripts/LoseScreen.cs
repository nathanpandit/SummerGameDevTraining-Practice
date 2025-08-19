using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : BaseScreen
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] public Button retryButton;
    
    void Awake()
    {
        type = ScreenType.LoseScreen;
        retryButton.onClick.AddListener(OnRetryButtonClicked);
    }

    void OnRetryButtonClicked()
    {
        EventManager.Instance().OnGameStart();
    }
}
