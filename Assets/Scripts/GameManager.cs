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
                ufos.Remove(ufo);
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

        private static bool IsPositionValid(Ufo ufo)
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
                    // empty for now
                    return true;
                }
            }
            return false;
        }

        private static void ClearHighLight()
        {
            
        }


        public static void HighLightTile(Ufo ufo)
        {

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