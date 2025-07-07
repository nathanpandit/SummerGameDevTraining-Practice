using TMPro;
using UnityEngine;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;

namespace UfoPuzzle
{
    public class Grid : MonoBehaviour
    {
        public Vector2Int position;
        public Renderer gridRenderer, circleRenderer;
        public Circle circlePrefab;
        public Circle circle;
        public bool exists = false;
        public bool isSlot = false;
        public int orderOfTrio = 0;

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
            circleRenderer = circle.circleRenderer;
            emptyGrid();
        }
        
        public void InitializeAsSlot(int i)
        {
            isSlot = true;
            name = $"Slot {i}";
            circle = Instantiate(circlePrefab, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            circle.transform.SetParent(transform);
            circleRenderer = circle.circleRenderer;
            emptyGrid();
        }

        public void setTile()
        {
            if (!circle.gameObject.activeSelf)
            {
                circle.gameObject.SetActive(true);
            }
            circleRenderer.material.color = LevelEditorA.Instance().colorDict[LevelEditorA.Instance().paintColor];
        }

        public void emptyGrid()
        {
            circle.gameObject.SetActive(false);
            gridRenderer.material.color = LevelEditorA.Instance().emptyGridColor;
        }

        public void delete(LevelData levelData)
        {
            if (levelData.tileData.Exists(x => x.position == position))
            {
                levelData.tileData.RemoveAll(x => x.position == position);
            }
            Destroy(gameObject);
        }
        
        
        
    }

}