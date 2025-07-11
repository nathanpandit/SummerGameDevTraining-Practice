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
        public static List<Tile> HighlightedTiles;

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
                Vector3 position = ufo.originalPos;
                ufos.Remove(ufo);
                ufo.delete();
                Debug.Log("Removed ufo");
                // EventManager.Instance.TriggerEvent(new BlockReleasedEvent(block.spawnTransform));
            }
            else
            {
                // If any position is invalid, return to original spawn position
                ufo.ResetPosition();
            }
            
            ClearHighLight();
        }

        public static bool IsPositionValid(Ufo ufo)
        {
            Vector2Int position = new Vector2Int(Mathf.RoundToInt(ufo.transform.position.x),
                Mathf.RoundToInt(ufo.transform.position.z));
            Debug.Log($"Looking for tile in {position}");
            if (tiles.Exists(x => x.position == position))
            {
                Debug.Log($"Found tile in {position}");
                Tile tile = tiles.FirstOrDefault(x => x.position == position);
                if (tile.circle.gameObject.activeSelf &&
                    tile.circleRenderer.material.color == ufo.ufoRenderer.material.color)
                {
                    HighLightCircle(tile, ufo.ufoRenderer.material.color);
                    return true;
                }
            }
            return false;
        }

        private static void ClearHighLight()
        {
            foreach (Tile _tile in HighlightedTiles)
            {
                _tile.circleRenderer.material.color = new Color(_tile.circleRenderer.material.color.r,
                    _tile.circleRenderer.material.color.g, _tile.circleRenderer.material.color.b, 1f);
                HighlightedTiles.Remove(_tile);
            }
        }


        public static void HighLightCircle(Tile _tile, Color color)
        {
            if (_tile.circleRenderer.material.color != color)
            {
                _tile.isVisited = true;
                return;
            }
            _tile.isVisited = true;
            _tile.circleRenderer.material.color = new Color(_tile.circleRenderer.material.color.r,
                _tile.circleRenderer.material.color.g, _tile.circleRenderer.material.color.b, 0.5f);
            HighlightedTiles.Add(_tile);
            List<Tile> allNeighbors = new List<Tile>();
            Vector2Int position = _tile.position;

            Debug.Log($"Found tile in {position}");
            Tile tile = tiles.FirstOrDefault(x => x.position == position);
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
    }
}