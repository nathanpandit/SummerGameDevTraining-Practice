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
            tileData = new List<TileData>(),
            exitData = new List<ExitData>(),
            blockData = new List<BlockData>()
        };
        
        private ObjectType objectType;
        private Movement movement;
        private ObjectColor objectColor;
        private Direction direction;
        private Grid referenceGrid = null;

        [Range(0,60)]
        public int count;

        [Range(-1, 300)]
        public int timeLimit;
        
        private bool hasKey;
        private bool isMetal;
        public ObjectMaterial material;

        public Vector2Int mapSize;
        public int level;

        public Transform tileParent;
        public Transform rulersParent;
        public Grid gridPrefab;
        public GameObject cubePrefab;
        public GameObject exitPrefab;
        public TextMeshPro rulerTextPrefab;

        public Color emptyGridColor;
        public Color tileGridColor;
        public Color obstacleGridColor;

        public Dictionary<ObjectColor, Color> colorDict = new Dictionary<ObjectColor, Color>
        {
            {ObjectColor.Colorless, new Color32(255, 255, 255, 255) },
            {ObjectColor.Red, new Color32(155, 53, 51,255)},
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
            if (Input.GetMouseButton(0) && !IsPointerOverUIObject())
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
                            TileData tileData = new TileData();
                            tileData.position = grid.position;
                            tileData.hasObstacle = false;
                            //if exist find and update data in list
                            if (levelData.tileData.Exists(x => x.position == tileData.position))
                            {
                                levelData.tileData.RemoveAll(x => x.position == tileData.position);
                            }
                            levelData.tileData.Add(tileData);
                            grid.setTile();
                        }
                        else if (objectType == ObjectType.Exit)
                        {
                            if (levelData.exitData.Exists(x => x.position == grid.position))
                            {
                                levelData.exitData.RemoveAll(x => x.position == grid.position);
                            }
                            ExitData exitData = new ExitData();
                            exitData.position = grid.position;
                            exitData.direction = direction;
                            exitData.exitColor = objectColor;
                            exitData.material = material;
                            levelData.exitData.Add(exitData);
                            grid.addExit(exitData, grid);
                        }
                        else if (objectType == ObjectType.Block)
                        {
                            if (!referenceGrid)
                            {
                                referenceGrid = grid;
                                BlockData blockData = new BlockData();
                                blockData.originPos = grid.position;
                                blockData.cubeData = new List<CubeData>();
                                blockData.movement = movement;
                                blockData.counter = count;
                                levelData.blockData.Add(blockData);
                            }
                            else
                            {
                                var blockData = levelData.blockData.FirstOrDefault(x => x.originPos == referenceGrid.position);
                                if (blockData != null)
                                {
                                    if (blockData.cubeData.Exists(x => x.position == grid.position))
                                    {
                                        blockData.cubeData.RemoveAll(x => x.position == grid.position);
                                    }
                                    CubeData cubeData = new CubeData();
                                    cubeData.position = grid.position;
                                    cubeData.objectColor = objectColor;
                                    cubeData.hasKey = hasKey;
                                    cubeData.material = material;
                                    blockData.cubeData.Add(cubeData);
                                    grid.addCube(cubeData, referenceGrid, blockData);
                                }
                            }
                        }
                        else if (objectType == ObjectType.Obstacle)
                        {
                            if (levelData.tileData.Exists(x => x.position == grid.position))
                            {
                                levelData.tileData.RemoveAll(x => x.position == grid.position);
                            }
                            TileData tileData = new TileData();
                            tileData.position = grid.position;
                            tileData.hasObstacle = true;
                            levelData.tileData.Add(tileData);

                            grid.addObstacle();
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

                        if (objectType == ObjectType.Block)
                        {

                            foreach (BlockData blockData in levelData.blockData)
                            {
                                blockData.cubeData.RemoveAll(x => x.position == grid.position);

                            }
                            grid.removeCube();

                            levelData.blockData.RemoveAll(x => x.cubeData.Count == 0);
                            

                            if((referenceGrid != null) && (!levelData.blockData.Any( x => x.originPos == referenceGrid.position)))
                            {
                                referenceGrid = null;

                            }
                        }
                        if (objectType == ObjectType.Exit)
                        {
                            levelData.exitData.RemoveAll(x => x.position == grid.position);

                            grid.removeExit();
                        }

                        if (objectType == ObjectType.Obstacle)
                        {
                            if(levelData.tileData.Exists(x=> x.position == grid.position))
                            {

                                if (levelData.tileData.Exists(x => x.position == grid.position))
                                {
                                    levelData.tileData.RemoveAll(x => x.position == grid.position);
                                }

                                TileData tileData = new TileData();
                                tileData.position = grid.position;
                                tileData.hasObstacle = false;
                                levelData.tileData.Add(tileData);

                                grid.removeObstacle();

                            }

                        }

                    }
                }
            }
            int key;
            if (Input.GetKeyDown(KeyCode.Keypad0)) count = 0;
            else if (Input.GetKeyDown(KeyCode.Keypad1)) count = 1;
            else if (Input.GetKeyDown(KeyCode.Keypad2)) count = 2;
            else if (Input.GetKeyDown(KeyCode.Keypad3)) count = 3;
            else if (Input.GetKeyDown(KeyCode.Keypad4)) count = 4;
            else if (Input.GetKeyDown(KeyCode.Keypad5)) count = 5;
            else if (Input.GetKeyDown(KeyCode.Keypad6)) count = 6;
            else if (Input.GetKeyDown(KeyCode.Keypad7)) count = 7;
            else if (Input.GetKeyDown(KeyCode.Keypad8)) count = 8;
            else if (Input.GetKeyDown(KeyCode.Keypad9))
            {

                count = 9;
            }
            else if (Input.GetKeyDown(KeyCode.S))
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
            else if (Input.GetKeyDown(KeyCode.T))
            {
                objectType = ObjectType.Tile;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                objectType = ObjectType.Exit;
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                referenceGrid = null;
                objectType = ObjectType.Block;
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                objectType = ObjectType.Obstacle;
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                movement = Movement.Horizontal;
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                movement = Movement.Vertical;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                movement = Movement.Free;
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                movement = Movement.Ice;
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                movement = Movement.Locked;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                hasKey = !hasKey;
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {

                int currentMaterial = (int)material;
                int materialCount = Enum.GetNames(typeof(ObjectMaterial)).Length;
                int nextMaterial = (currentMaterial + 1) % materialCount;

                material = (ObjectMaterial)nextMaterial;
                
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Direction.Up;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Direction.Right;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Direction.Down;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Direction.Left;
            }
            else if (int.TryParse(Input.inputString, out key))
            {
                objectColor = (ObjectColor)(key - 1);

                if (isMetal)
                {
                    string metalName = "Metal" + objectColor.ToString();
                    if (Enum.TryParse(metalName, out objectColor))
                    {
                        Debug.Log($"Metalization successful for {objectColor.ToString()}");
                    }
                    else
                    {
                        Debug.Log($"Couldn't find the color '{metalName}'");
                    }
                }

                Debug.Log($"Color name is {objectColor.ToString()}");

            }
            else if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                count++;
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                count--;
            }
        }
        
        private void GenerateGrid()
        {
            if (tileParent) UnityEngine.Object.Destroy(tileParent.gameObject);
            tileParent = new GameObject("TileParent").transform;
            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    Grid grid = UnityEngine.Object.Instantiate(gridPrefab, new Vector3(i, 0, j), Quaternion.identity, parent: tileParent);
                    grid.Initialize(new Vector2Int(i, j));
                }
            }

            levelData = new LevelData()
            {
                tileData = new List<TileData>(),
                exitData = new List<ExitData>(),
                blockData = new List<BlockData>()
            };

            AddRulers();

            ///CAMERA
            ///

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
            GenerateGrid();
            
            TextAsset jsonFile = Resources.Load<TextAsset>($"Levels/Level_{level}");
            levelData = JsonUtility.FromJson<LevelData>(jsonFile.text);
            Debug.Log($"Level {level} loaded:\n{jsonFile.text}");

            timeLimit = levelData.timeLimit;
            
            var cells = tileParent.GetComponentsInChildren<Grid>();
            foreach (var tile in levelData.tileData)
            {
                var grid = cells.FirstOrDefault(x => x.position == tile.position);
                if (grid != null)
                {
                    if (tile.hasObstacle)
                    {
                        grid.addObstacle();
                    }
                    else
                    {
                        grid.setTile();
                    }
                }
            }
            foreach (var block in levelData.blockData)
            {
                var referance = cells.FirstOrDefault(x => x.position == block.originPos);
                foreach (var cube in block.cubeData)
                {
                    var grid = cells.FirstOrDefault(x => x.position == cube.position);
                    if (grid != null)
                    {
                        grid.addCube(cube, referance, block);
                    }
                }
            }
            foreach (var exit in levelData.exitData)
            {
                var grid = cells.FirstOrDefault(x => x.position == exit.position);
                if (grid != null)
                {
                    grid.addExit(exit, grid);
                }
            }
        }


        void AddRulers()
        {
            if(rulersParent) { UnityEngine.Object.Destroy(rulersParent.gameObject); }
            rulersParent =  new GameObject("RulerParent").transform;

            for(int x = 0; x < mapSize.x; x++)
            {
                TextMeshPro number = UnityEngine.Object.Instantiate(rulerTextPrefab, position: new Vector3(x + 1, 0, -1), Quaternion.identity, rulersParent);
                number.SetText(x.ToString());
                number.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                number.gameObject.name = $"X: {x}";
            }
            for(int y = 0; y < mapSize.y; y++)
            {
                TextMeshPro number = UnityEngine.Object.Instantiate(rulerTextPrefab, position: new Vector3(0, 0, y), Quaternion.identity, rulersParent);
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
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}