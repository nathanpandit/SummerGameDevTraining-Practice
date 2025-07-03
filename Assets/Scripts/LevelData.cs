using System;
using System.Collections.Generic;
using UnityEngine;

namespace WoodPuzzle
{
    [Serializable]
    public class LevelData
    {
        public List<TileData> tileData = new List<TileData>();
        public int timeLimit;
        public Vector2Int sizeOfLevel;
    }
    

    [Serializable]
    public class TileData
    {
        public Vector2Int position;
        public Color color;
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
    [Serializable]
    public enum ObjectMaterial
    {
        Wood,
        Metal
    }
}