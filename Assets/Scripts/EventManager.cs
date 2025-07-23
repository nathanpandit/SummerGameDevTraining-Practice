using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public ScreenManager screenManager;
    public event UnityAction LevelLost;
    public event UnityAction LevelWon;
    public void OnLevelLost() => LevelLost?.Invoke();
    public void OnLevelWon() => LevelWon?.Invoke();
}
