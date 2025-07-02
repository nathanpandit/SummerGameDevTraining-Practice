using System;
using System.Collections.Generic;
using UnityEngine;

namespace WoodPuzzle
{
    [Serializable]
    public class LevelData
    {
        public List<TileData> tileData = new List<TileData>();
        public List<ExitData> exitData = new List<ExitData>();
        public List<BlockData> blockData = new List<BlockData>();
        public int timeLimit;
    }
    

    [Serializable]
    public class TileData
    {
        public Vector2Int position;
        public bool hasObstacle;
    }

    [Serializable]
    public class ExitData
    {
        public ObjectMaterial material;
        public Vector2Int position;
        public Direction direction;
        public ObjectColor exitColor;
    }
    
    [Serializable]
    public class BlockData
    {
        public Vector2Int originPos;
        public List<CubeData> cubeData = new List<CubeData>(); 
        public Movement movement;
        public int counter;
    }
    
    [Serializable]
    public class CubeData
    {
        public bool hasKey;
        public ObjectMaterial material;
        public Vector2Int position;
        public ObjectColor objectColor;
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
    public enum Movement
    {
        Free,
        Horizontal,
        Vertical,
        Ice,
        Locked,
    }
    [Serializable]
    public enum ObjectType
    {
        Tile,
        Block,
        Exit,
        Obstacle
    }
    [Serializable]
    public enum ObjectMaterial
    {
        Wood,
        Metal
    }
}