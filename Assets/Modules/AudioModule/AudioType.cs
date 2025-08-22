using UnityEngine;

public enum SoundType
{
    Win,
    Lose,
    Coin,
    None
}

public enum MusicType
{
    StartMenu,
    Game,
    WinScreen,
    None
}


[System.Serializable]
public class Audio
{
    public SoundType soundType;
    public MusicType musicType;
    public AudioClip clip;
}