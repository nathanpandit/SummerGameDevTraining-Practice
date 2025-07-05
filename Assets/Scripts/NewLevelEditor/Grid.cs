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
        public Renderer gridRenderer, circleRenderer;
        public GameObject circlePrefab;
        public GameObject circle;

        private void Awake()
        {
            gridRenderer = GetComponent<Renderer>();
        }

        public void Initialize(Vector2Int pos)
        {
            position = pos;
            transform.position = new Vector3(pos.x, 0, pos.y);
            name = "Grid " + position.x + " " + position.y;
            circle = Instantiate(circlePrefab, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            circle.transform.SetParent(transform);
            circleRenderer = circle.GetComponent<Renderer>();
            emptyGrid();
        }

        public void setTile()
        {
            if (!circle.activeSelf)
            {
                circle.SetActive(true);
            }
            circleRenderer.material.color = LevelEditorA.Instance().colorDict[LevelEditorA.Instance().tileGridColor];
        }

        public void emptyGrid()
        {
            circle.SetActive(false);
            gridRenderer.material.color = LevelEditorA.Instance().emptyGridColor;
        }
        
        
        
    }

}