using UfoPuzzle;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : BaseScreen
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Button startGameButton;

    void Awake()
    {
        type = ScreenType.StartScreen;
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    void OnEnable()
    {
        AudioManager.Instance().PlayMusic(MusicType.StartMenu);
    }

    void OnDisable()
    {
        AudioManager.Instance().StopPlayingMusic();
    }

    public void OnStartGameButtonClicked()
    {
        ScreenManager.Instance().HideScreen(ScreenType.StartScreen);
    }
}
