using System.Collections;
using System.Collections.Generic;
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
            if (IsPositionValid(ufo))
            {
                // If all positions are valid, snap to the rounded position
                ufo.isPlaced = true;

                ufos.Remove(ufo);
                EventManager.Instance.TriggerEvent(new BlockReleasedEvent(block.spawnTransform));
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
            return true;
        }

        private static void ClearHighLight()
        {
            
        }


        public static void HighLightTile(Ufo ufo)
        {

        }

        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}