using UnityEngine.Events;

public static class EventManager
{
    public static event UnityAction LevelLost;
    public static event UnityAction LevelWon;
    public static void OnLevelLost() => LevelLost?.Invoke();
    public static void OnLevelWon() => LevelWon?.Invoke();
}
