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
        private GameObject tutorialObject => Resources.Load<GameObject>("TutorialObject");
        public GameObject levelParent;
        public Ufo ufoPrefab;
        private int level = 1;

        public UfoSpawner ufoSpawner;

        private void Start()
        {
            Application.targetFrameRate = 60;
            ReadLevelData();
            var data = GenerateLevel();
            GameManager.Initialize(data.Item1, data.Item2, data.Item3);

            /*if (levelHelper.GetCurrentLevel() == 1)
            {
                Instantiate(tutorialObject, new Vector3(0, 0, 0), Quaternion.identity);
            } */
            // DragHelper.Initialize(data.tiles);

            EventManager.Instance().LevelLost += GameManager.EventManagerOnLevelLost;
            EventManager.Instance().LevelWon += GameManager.EventManagerOnLevelWon;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                LevelHelper.currentLevel++;
                level++;
                ReadLevelData();
                var data = GenerateLevel();
                GameManager.Initialize(data.Item1, data.Item2, data.Item3);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                LevelHelper.currentLevel--;
                level--;
                ReadLevelData();
                var data = GenerateLevel();
                GameManager.Initialize(data.Item1, data.Item2, data.Item3);
            }

            if (LevelHelper.currentLevel > level)
            {
                level++;
                ReadLevelData();
                var data = GenerateLevel();
                GameManager.Initialize(data.Item1, data.Item2, data.Item3);
            }
            else if (LevelHelper.currentLevel < level)
            {
                LevelHelper.NextLevel();
                ReadLevelData();
                var data = GenerateLevel();
                GameManager.Initialize(data.Item1, data.Item2, data.Item3);
            }
        }
        
        private void ReadLevelData()
        {
            var levelIndex = LevelHelper.GetCurrentLevel();
            string json = Resources.Load<TextAsset>($"Levels/Level_{levelIndex}").text;
            levelData = JsonUtility.FromJson<LevelData>(json);
        }
        
        private (List<Tile>, List<Circle>, List<Ufo>) GenerateLevel()
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
            var circles = new List<Circle>();
            var ufos = new List<Ufo>();
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
                    currentTile.circle.color = circleData.color;
                    circles.Add(currentTile.circle);
                }
                else
                {
                    Debug.Log("why is this tile null");
                }
            }
            
            ufos = ufoSpawner.Initialize(levelData.ufoData); ;

            return (tiles, circles, ufos);
        }

    }
}