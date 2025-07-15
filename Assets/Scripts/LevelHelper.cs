using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public static class LevelHelper
{
    public static int currentLevel = 1;
    public static bool circlesCleared;
    public static bool thereAreUfos;
    
    public static int GetCurrentLevel()
    {
        return currentLevel;
    }

    public static bool IsGameLost(List<Ufo> currentUfos, List<Circle> currentCircles)
    {
        return (!circlesCleared && !thereAreUfos) || (!circlesCleared  && !CanRemainingUfosWin(currentUfos, currentCircles));
    }

    public static bool CanRemainingUfosWin(List<Ufo> currentUfos, List<Circle> currentCircles)
    {
        foreach (Ufo ufo in currentUfos)
        {
            if (!currentCircles.Exists(x => x.color == ufo.color))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsLevelWon()
    {
        return circlesCleared;
    }
}
