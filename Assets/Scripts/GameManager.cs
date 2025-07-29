using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UfoPuzzle
{
    public static class GameManager
    {
        public static List<Tile> tiles = new List<Tile>();
        public static List<Circle> circles = new List<Circle>();
        public static List<Ufo> ufos = new List<Ufo>();
        public static List<Tile> HighlightedTiles = new List<Tile>();
        public static List<Tile> VisitedTiles = new List<Tile>();

        public static void Initialize(List<Tile> _tiles, List<Circle> _circles, List<Ufo> _ufos)
        {
            tiles = _tiles;
            circles = _circles;
            ufos = _ufos;
        }
        
        public static void HandleUfoRelease(Ufo ufo)
        {
            ufo.transform.localScale = new Vector3(0.9f, 0.01f, 0.9f);
            if (IsPositionValid(ufo))
            {
                // If all positions are valid, snap to the rounded position
                ufo.gameObject.SetActive(false);
                int nextIndex = ufos.IndexOf(ufo) + 3;
                foreach (Tile t in HighlightedTiles)
                {
                    t.circle.gameObject.SetActive(false);
                    circles.Remove(t.circle);
                }
                HighlightedTiles.Clear();
                if (nextIndex < ufos.Count)
                {
                    Ufo newUfo = ufos[nextIndex];
                    newUfo.transform.position = ufo.originalPos;
                    ufos[nextIndex].gameObject.SetActive(true);
                }
                else
                {
                    int numberOfUfos = ufos.Count(x => x.gameObject.activeSelf == true);
                    if (numberOfUfos == 0)
                    {
                        LevelHelper.thereAreUfos = false;
                    }
                }

                if (circles.Count() == 0)
                {
                    LevelHelper.circlesCleared = true;
                }
                if (LevelHelper.IsLevelWon())
                {
                    EventManager.Instance().OnLevelWon();
                }
                else
                {
                    List<Ufo> currentUfos = ufos.FindAll(x => x.gameObject.activeSelf == true);
                    if (LevelHelper.IsGameLost(currentUfos, circles))
                    {
                        EventManager.Instance().OnLevelLost();
                    }
                }

            }
            else
            {
                // If any position is invalid, return to original spawn position
                ufo.ResetPosition();
            }
            
        }

        public static bool IsPositionValid(Ufo ufo)
        {
            Vector2Int position = new Vector2Int(Mathf.RoundToInt(ufo.transform.position.x),
                Mathf.RoundToInt(ufo.transform.position.z));
            if (tiles.Exists(x => x.position == position))
            {
                Tile tile = tiles.FirstOrDefault(x => x.position == position);
                if ((tile.circle.gameObject.activeSelf &&
                    tile.circle.color == ufo.color) || !tile.circle.gameObject.activeSelf)
                {
                    HighLightCircle(tile, ufo.color);
                    foreach (Tile t in VisitedTiles)
                    {
                        t.isVisited = false;
                    }
                    VisitedTiles.Clear();
                    return true;
                }
            }
            ClearHighLight();
            return false;
        }

        private static void ClearHighLight()
        {
            foreach (Tile _tile in HighlightedTiles)
            {
                _tile.circleRenderer.material.color = _tile.circle.color;
            }

            HighlightedTiles.Clear();
        }


        public static void HighLightCircle(Tile _tile, Color color)
        {
            _tile.isVisited = true;
            VisitedTiles.Add(_tile);
            if (_tile.circle.gameObject.activeSelf && _tile.circle.color != color)
            {
                return;
            }
            else if (_tile.circle.gameObject.activeSelf)
            {
                HighlightedTiles.Add(_tile);
                _tile.circleRenderer.material.color = Color.black;
            }
            List<Tile> allNeighbors = new List<Tile>();
            Vector2Int position = _tile.position;

            Tile tile0 = tiles.FirstOrDefault(x => x.position == new Vector2Int(position.x + 1, position.y));
            if(tile0 != null && !tile0.isVisited) allNeighbors.Add(tile0);
            Tile tile1 = tiles.FirstOrDefault(x => x.position == new Vector2Int(position.x, position.y - 1));
            if(tile1 != null && !tile1.isVisited) allNeighbors.Add(tile1);
            Tile tile2 = tiles.FirstOrDefault(x => x.position == new Vector2Int(position.x - 1, position.y));
            if(tile2 != null && !tile2.isVisited) allNeighbors.Add(tile2);
            Tile tile3 = tiles.FirstOrDefault(x => x.position == new Vector2Int(position.x, position.y+1));
            if(tile3 != null && !tile3.isVisited) allNeighbors.Add(tile3);
            foreach (Tile t in allNeighbors)
            {
                HighLightCircle(t, color);
            }
        }

        /*
        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        */

        public static void EventManagerOnLevelLost()
        {
            Debug.Log($"You lost level {LevelHelper.GetCurrentLevel()}. Try again.");
            LevelHelper.currentLevel--;
        }

        public static void EventManagerOnLevelWon()
        {
            Debug.Log($"Congratulations! You won level {LevelHelper.GetCurrentLevel()}!");
            ScreenManager.Instance().SetCurrentScreen(ScreenType.WinScreen);
            LevelHelper.NextLevel();
        }
    }
}