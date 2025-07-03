using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

namespace WoodPuzzle
{
    public class LevelEditorA : Singleton<LevelEditorA>
    {

        private LevelData levelData = new LevelData()
        {
            tileData = new List<TileData>()
        };
        
        private ObjectType objectType;
        private ObjectColor objectColor;

        [Range(-1, 300)]
        public int timeLimit;
        
        private bool isMetal;
        public ObjectMaterial material;

        public Vector2Int mapSize;
        public int level;

        public Transform tileParent;
        public Transform rulersParent;
        public Grid gridPrefab;
        public TextMeshPro rulerTextPrefab;

        public Color emptyGridColor;
        public ObjectColor tileGridColor;

        public Dictionary<ObjectColor, Color> colorDict = new Dictionary<ObjectColor, Color>
        {
            {ObjectColor.Colorless, new Color32(255, 255, 255, 255) },
            {ObjectColor.Red, new Color32(255, 0, 0,255)},
            { ObjectColor.Blue, new Color32(60, 136, 170,255)},
            {ObjectColor.Green, Color.green },
            {ObjectColor.Yellow, Color.yellow },
            { ObjectColor.Purple, new Color32(160, 32, 240,255)},
            { ObjectColor.Pink, new Color32( 255, 105, 180, 255)},
            { ObjectColor.DarkBlue, new Color32(0, 0, 139, 255)},
            { ObjectColor.Brown, new Color32(150, 75, 0, 255)},
        };

        private void Start()
        {
            GenerateGrid();
        }
        
        
        private void Update()
        {
            if (Input.GetMouseButton(0) && !IsPointerOverUIObject() )
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Grid grid = hit.collider.GetComponent<Grid>();
                    if (grid != null)
                    {
                        if (objectType == ObjectType.Tile)
                        {
                            grid.setTile();
                            TileData tileData = new TileData();
                            tileData.position = grid.position;
                            tileData.color = grid.gridRenderer.material.color;
                            //if exist find and update data in list
                            if (levelData.tileData.Exists(x => x.position == tileData.position))
                            {
                                levelData.tileData.RemoveAll(x => x.position == tileData.position);
                            }
                            levelData.tileData.Add(tileData);
                        }
                    }
                }
            }
            else if (Input.GetMouseButton(1) && !IsPointerOverUIObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Grid grid = hit.collider.GetComponent<Grid>();
                    if (grid != null)
                    {
                        if (objectType == ObjectType.Tile)
                        {
                            levelData.tileData.RemoveAll(x => x.position == grid.position);
                            grid.emptyGrid();
                        }
                    }
                }
            }
            int key;

            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveData();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                LoadLevel();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                ResetLevel();
            }
            /* currently not using any materials
            else if (Input.GetKeyDown(KeyCode.M))
            {
                int currentMaterial = (int)material;
                int materialCount = Enum.GetNames(typeof(ObjectMaterial)).Length;
                int nextMaterial = (currentMaterial + 1) % materialCount;

                material = (ObjectMaterial)nextMaterial;
                
            }
            */
            else if (int.TryParse(Input.inputString, out key))
            {
                objectColor = (ObjectColor)(key - 1);
                Debug.Log($"Color name is {objectColor.ToString()}");
                tileGridColor = objectColor;

            }
        }
        
        private void GenerateGrid()
        {
            if (tileParent) Destroy(tileParent.gameObject);
            tileParent = new GameObject("TileParent").transform;
            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    Grid grid = Instantiate(gridPrefab, new Vector3(i, 0, j), Quaternion.identity, parent: tileParent);
                    grid.Initialize(new Vector2Int(i, j));
                }
            }

            levelData = new LevelData()
            {
                tileData = new List<TileData>(),
                sizeOfLevel = mapSize
            };

            AddRulers();
            
            /* Camera */

            Vector3 cameraCenter = new Vector3(mapSize.x/ 2, 5, (mapSize.y/ 2) - 1);

            Camera.main.transform.position = cameraCenter;

            Debug.Log($"X mapsize: {(mapSize.x + 1) / 2}");
            Debug.Log($"X size: {(mapSize.x + 1) / 2}");
            Debug.Log($"Y size: {(mapSize.y + 1) / (2)}");

            Camera.main.orthographicSize = Mathf.Max((float)(mapSize.x + 1) / 2, (float)(mapSize.y + 1)/ (2));
        }
        
        public void SaveData()
        {
            levelData.timeLimit = timeLimit;
            string saveData = JsonUtility.ToJson(levelData);
            string filePath = Application.dataPath + $"/Resources/Levels/Level_{level}.json";
            System.IO.File.WriteAllText(filePath, saveData);
            Debug.Log($"Level {level} saved:\n{saveData}");

            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }

        public void LoadLevel()
        {
            //Generate Level From LevelData
            TextAsset jsonFile = Resources.Load<TextAsset>($"Levels/Level_{level}");
            if (!jsonFile)
            {
                Debug.Log("JSON file is null");
            }
            levelData = JsonUtility.FromJson<LevelData>(jsonFile.text);
            mapSize = levelData.sizeOfLevel;
            LevelData tempData = levelData;
            GenerateGrid();
            levelData = tempData;
            Debug.Log($"Level {level} loaded:\n{jsonFile.text}");

            timeLimit = levelData.timeLimit;
            var cells = tileParent.GetComponentsInChildren<Grid>();
            foreach (var tile in levelData.tileData)
            {
                var grid = cells.FirstOrDefault(x => x.position == tile.position);
                if (grid != null)
                {
                    grid.gridRenderer.material.color = tile.color;
                }
            }
        }


        void AddRulers()
        {
            if(rulersParent) { Destroy(rulersParent.gameObject); }
            rulersParent =  new GameObject("RulerParent").transform;

            for(int x = 0; x < mapSize.x; x++)
            {
                TextMeshPro number = Instantiate(rulerTextPrefab, position: new Vector3(x + 1, 0, -1), Quaternion.identity, rulersParent);
                number.SetText(x.ToString());
                number.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                number.gameObject.name = $"X: {x}";
            }
            for(int y = 0; y < mapSize.y; y++)
            {
                TextMeshPro number = Instantiate(rulerTextPrefab, position: new Vector3(0, 0, y), Quaternion.identity, rulersParent);
                number.SetText(y.ToString());
                number.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                number.gameObject.name = $"Y: {y}";
            }
        }

        public void ResetLevel()
        {
            GenerateGrid();
        }
        
        private bool IsPointerOverUIObject() {
            if (EventSystem.current == null)
            {
                Debug.Log("Event system does not exist!");
                return false;
            }
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}