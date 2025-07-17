using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

namespace UfoPuzzle
{
    public static class LevelHelper
    {
        public static int currentLevel = 1;
        public static bool circlesCleared;
        public static bool thereAreUfos = true;

        public static int GetCurrentLevel()
        {
            return currentLevel;
        }

        public static bool IsGameLost(List<Ufo> currentUfos, List<Circle> currentCircles)
        {
            Debug.Log("Circles cleared: " + circlesCleared);
            Debug.Log("There are ufos: " + thereAreUfos);
            Debug.Log("Can remaining ufos win: " + CanRemainingUfosWin(currentUfos, currentCircles));
            return (!circlesCleared && !thereAreUfos) ||
                   (!circlesCleared && !CanRemainingUfosWin(currentUfos, currentCircles));
        }

        public static bool CanRemainingUfosWin(List<Ufo> currentUfos, List<Circle> currentCircles)
        {
            foreach (Ufo ufo in currentUfos)
            {
                if (currentCircles.Exists(x => x.color == ufo.color))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsLevelWon()
        {
            return circlesCleared;
        }

        public static void NextLevel()
        {
            currentLevel += 1;
            circlesCleared = false;
            thereAreUfos = true;
        }
    }
}
