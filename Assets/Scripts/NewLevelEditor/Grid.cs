using TMPro;
using UnityEngine;
using TMPro;
using System.Collections;
using System;

namespace WoodPuzzle
{
    public class Grid : MonoBehaviour
    {
        public Vector2Int position;
        public Renderer gridRenderer;
        
        GameObject exit;

        private void Awake()
        {
            gridRenderer = GetComponent<Renderer>();
        }

        public void Initialize(Vector2Int pos)
        {
            position = pos;
            transform.position = new Vector3(pos.x, 0, pos.y);
            name = "Grid " + position.x + " " + position.y;
            emptyGrid();
        }

        public void setTile()
        {
            gridRenderer.material.color = LevelEditorA.Instance().colorDict[LevelEditorA.Instance().tileGridColor];
        }

        public void emptyGrid()
        {
            gridRenderer.material.color = LevelEditorA.Instance().emptyGridColor;
        }
        
        
        
    }

}