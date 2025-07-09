using System;
using System.Collections.Generic;
using UnityEngine;

namespace UfoPuzzle
{
    [Serializable]
    public class LevelData
    {
        public List<TileData> tileData = new List<TileData>();
        public List<CircleData> circleData = new List<CircleData>();
        public List<UfoData> ufoData = new List<UfoData>();
        public Vector2Int sizeOfLevel;
    }
    

    [Serializable]
    public class TileData
    {
        public Vector2Int position;
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
    public class CircleData
    {
        public Vector2Int position;
        public Color color;
    }

    [Serializable]
    public class UfoData
    {
        public Color color;
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
        Tile,
        Circle,
        Ufo
    }
}