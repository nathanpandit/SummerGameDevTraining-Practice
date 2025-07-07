using System;
using System.Collections.Generic;
using UnityEngine;

namespace UfoPuzzle
{
    [Serializable]
    public class LevelData
    {
        public List<TileData> tileData = new List<TileData>();
        public List<SlotData> slotData = new List<SlotData>();
        public Vector2Int sizeOfLevel;
    }
    

    [Serializable]
    public class TileData
    {
        public Vector2Int position;
        public Color color;
        public bool isActive;
    }

    [Serializable]
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    [Serializable]
    public class SlotData
    {
        public int orderOfTrio;
        public Color color0, color1, color2;
    }

    [Serializable]
    public enum ObjectColor
    {
        Red,
        Blue,
        Green,
        Yellow,
        Purple,
        Pink,
        DarkBlue,
        Brown,
        Colorless
    }
    [Serializable]
    public enum ObjectType
    {
        Tile
    }
}