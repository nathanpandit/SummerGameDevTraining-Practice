using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace UfoPuzzle
{
    public class LevelGenerator : MonoBehaviour
    {
        private LevelData levelData;
        public Tile tilePrefab;
        public Slot slotPrefab;
        public LevelHelper levelHelper;
        private GameObject tutorialObject => Resources.Load<GameObject>("TutorialObject");
        public int level;
        public GameObject levelParent;
        public Ufo ufoPrefab;

        public UfoSpawner ufoSpawner;

        private void Start()
        {
            Application.targetFrameRate = 60;
            ReadLevelData();
            var data = GenerateLevel();

            /*if (levelHelper.GetCurrentLevel() == 1)
            {
                Instantiate(tutorialObject, new Vector3(0, 0, 0), Quaternion.identity);
            } */
            // DragHelper.Initialize(data.tiles);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                level++; 
                ReadLevelData();
                var data = GenerateLevel();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                level--;
                ReadLevelData();
                var data = GenerateLevel();
            }
        }
        
        private void ReadLevelData()
        {
            var levelIndex = level;
            string json = Resources.Load<TextAsset>($"Levels/Level_{levelIndex}").text;
            levelData = JsonUtility.FromJson<LevelData>(json);
        }
        
        private List<Tile> GenerateLevel()
        {
            if (levelParent)
            {
                Destroy(levelParent);
            }
            levelParent = new GameObject("Level");


            /* adjust camera */
            Vector3 cameraCenter = new Vector3((float)levelData.sizeOfLevel.x/2 - 0.5f, 5, (float)levelData.sizeOfLevel.y/2 - 0.5f);
            Camera.main.transform.position = cameraCenter;
            Camera.main.orthographicSize = Mathf.Max((float)(levelData.sizeOfLevel.x + 1) / 2, (float)(levelData.sizeOfLevel.y + 1)/ (2)) * 1.5f;

            var tileParent = new GameObject("Tiles");
            tileParent.transform.SetParent(levelParent.transform);
            var tiles = new List<Tile>();
            foreach (var tileData in levelData.tileData)
            {
                var tile = Instantiate(tilePrefab, new Vector3(tileData.position.x, 0, tileData.position.y), Quaternion.identity, tileParent.transform);
                tile.Initialize(tileData);
                tiles.Add(tile);
            }

            foreach (CircleData circleData in levelData.circleData)
            {
                Tile currentTile = tiles.Find(x => x.position == circleData.position);
                if (currentTile != null)
                {
                    currentTile.circle.gameObject.SetActive(true);
                    currentTile.circleRenderer.material.color = circleData.color;
                }
                else
                {
                    Debug.Log("why is this tile null");
                }
            }
            
            ufoSpawner.Initialize(levelData.ufoData);

            return tiles;
        }

    }
}