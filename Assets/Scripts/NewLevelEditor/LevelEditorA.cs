using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

namespace UfoPuzzle
{
    public class LevelEditorA : Singleton<LevelEditorA>
    {
        public Vector2Int mapSize;

        private LevelData levelData = new LevelData()
        {
            tileData = new List<TileData>(),
            circleData = new List<CircleData>(),
            ufoData = new List<UfoData>()
        };
        
        private ObjectType objectType;
        private ObjectColor objectColor;

        [Range(-1, 300)]
        
        private bool isMetal;

        public int level;

        public Transform tileParent;
        public Transform rulersParent;
        public Grid gridPrefab;
        public TextMeshPro rulerTextPrefab;

        public Color emptyGridColor;
        public ObjectColor paintColor;

        public Stack<Grid> ufoCol0;
        public Stack<Grid> ufoCol1;
        public Stack<Grid> ufoCol2;
        private int numberOfUfoTrios;
        public bool addUfoTrio, removeUfoTrio;
        public GameObject ufoEditor;
        public UfoTrio trioPrefab;
        private float editorScale;
        private float scroll;

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
            ufoEditor = new GameObject("Ufo Editor");
            addUfoTrio = false;
            removeUfoTrio = false;
            numberOfUfoTrios = 0;
            ufoCol0 = new Stack<Grid>();
            ufoCol1 = new Stack<Grid>();
            ufoCol2 = new Stack<Grid>();
            GenerateGrid();
            gameObject.transform.position = new Vector3(mapSize.x + 1.45f, 0f, mapSize.y - 0.55f);
            editorScale = (float)Mathf.Max(mapSize.x, mapSize.y) / (Mathf.Max(mapSize.x, mapSize.y) + 2);
        }
        
        
        private void Update()
        {
            if (addUfoTrio)
            {
                addUfoTrio = false;
                numberOfUfoTrios++;
                handleStack();
            }

            if (removeUfoTrio)
            {
                removeUfoTrio = false;
                if(numberOfUfoTrios > 0) numberOfUfoTrios--;
                handleStack();
            }
            
            scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0f)
            {
                Camera.main.transform.position += new Vector3(0f, 0f, scroll);
            }

            if (Input.GetMouseButton(0) && !IsPointerOverUIObject() )
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Grid grid = hit.collider.GetComponent<Grid>();
                    if (grid != null && grid.objectType == ObjectType.Circle)
                    {
                        if (objectType != ObjectType.Tile)
                        {
                            grid.setTile();
                            if (!grid.isSlot)
                            {
                                TileData tileData = new TileData();
                                CircleData circleData = new CircleData();
                                tileData.position = grid.position;
                                circleData.position = grid.position;
                                circleData.color = grid.circleRenderer.material.color;
                                // tileData.isActive = true;
                                //if exist find and update data in list
                                if (levelData.tileData.Exists(x => x.position == tileData.position))
                                {
                                    levelData.tileData.RemoveAll(x => x.position == tileData.position);
                                }

                                if (levelData.circleData.Exists(x => x.position == circleData.position))
                                {
                                    levelData.circleData.RemoveAll(x => x.position == circleData.position);
                                }
                                levelData.tileData.Add(tileData);
                                levelData.circleData.Add(circleData);
                            }
                            else
                            {
                                UfoData ufoData = new UfoData();
                                UfoData pastData = levelData.ufoData.Find(x => x.orderOfTrio == grid.orderOfTrio);
                                levelData.slotData.Remove(pastData);
                                slotData.orderOfTrio = pastData.orderOfTrio;
                                levelData.slotData.Add(slotData);
                            }
                        }
                    }
                }
                else
                {
                    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                    if (groundPlane.Raycast(ray, out float enter))
                    {
                        Vector3 worldPosition = ray.GetPoint(enter);
                        Debug.Log("Mouse clicked at: " + worldPosition);
                        int finalX = Mathf.RoundToInt(worldPosition.x);
                        int finalZ = Mathf.RoundToInt(worldPosition.z);
                        Vector2Int newGridPos = new Vector2Int(finalX, finalZ);
                        if (-0.45f <= finalX && finalX <= mapSize.x-0.55f && -0.45f <= finalZ && finalZ <= mapSize.y-0.55f && !levelData.tileData.Exists(x => x.position == new Vector2Int(finalX, finalZ)))
                        {
                            Grid newGrid = Instantiate(gridPrefab, Vector3.zero, quaternion.identity);
                            newGrid.Initialize(newGridPos);
                            newGrid.gameObject.transform.SetParent(tileParent.transform);
                            TileData tileData = new TileData();
                            CircleData circleData = new CircleData();
                            tileData.position = new Vector2Int(finalX, finalZ);
                            circleData.position = new Vector2Int(finalX, finalZ);
                            circleData.color = emptyGridColor;
                            // tileData.isActive = false;
                            levelData.tileData.Add(tileData);
                            levelData.circleData.Add(circleData);
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
                        levelData.tileData.RemoveAll(x => x.position == grid.position);
                        if (grid.objectType == ObjectType.Circle) 
                        {
                                grid.emptyGrid();
                                TileData tileData = new TileData();
                                CircleData circleData = new CircleData();
                                circleData.color = grid.circleRenderer.material.color;
                                circleData.position = grid.position;
                                tileData.position = grid.position;
                                // tileData.isActive = false;
                                levelData.tileData.Add(tileData);
                                levelData.circleData.Add(circleData);
                        }
                        else if (!grid.isSlot && grid.objectType == ObjectType.Tile)
                        {
                                grid.delete(levelData);
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
            else if (int.TryParse(Input.inputString, out key))
            {
                objectColor = (ObjectColor)(key - 1);
                paintColor = objectColor;

            }
        }
        
        private void GenerateGrid()
        {
            if (tileParent) Destroy(tileParent.gameObject);
            tileParent = new GameObject("TileParent").transform;
            levelData = new LevelData()
            {
                tileData = new List<TileData>(),
                ufoData = new List<UfoData>(),
                circleData = new List<CircleData>(),
                sizeOfLevel = mapSize
            };
            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    Grid grid = Instantiate(gridPrefab, new Vector3(i, 0, j), Quaternion.identity, parent: tileParent);
                    grid.Initialize(new Vector2Int(i, j));
                    TileData newTileData = new TileData();
                    newTileData.position = grid.position;
                    levelData.tileData.Add(newTileData);
                }
            }



            AddRulers();
            
            /* Camera */

            Vector3 cameraCenter = new Vector3((float)mapSize.x/ 2, 5, (float)mapSize.y/2 - 1);

            Camera.main.transform.position = cameraCenter;

            Debug.Log($"X mapsize: {(mapSize.x + 1) / 2}");
            Debug.Log($"X size: {(mapSize.x + 1) / 2}");
            Debug.Log($"Y size: {(mapSize.y + 1) / (2)}");

            Camera.main.orthographicSize = Mathf.Max((float)(mapSize.x + 1) / 2, (float)(mapSize.y + 1)/ (2)) * 1.5f;
        }
        
        public void SaveData()
        {
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
                Debug.Log("Creating new level...");
                ResetSlots();
                GenerateGrid();
                Debug.Log("New level generated!");
                gameObject.transform.position = new Vector3(mapSize.x + 1.45f, 0f, mapSize.y - 0.55f);
            }
            else
            {
                levelData = JsonUtility.FromJson<LevelData>(jsonFile.text);
                mapSize = levelData.sizeOfLevel; 
                gameObject.transform.position = new Vector3(mapSize.x + 1.45f, 0f, mapSize.y - 0.55f);

                LevelData tempData = levelData;
                ResetSlots();
                GenerateGrid();
                levelData = tempData;
                Debug.Log($"Level {level} loaded:\n{jsonFile.text}");
            }

            var cells = tileParent.GetComponentsInChildren<Grid>();
            foreach (var tile in levelData.tileData)
            {
                var grid = cells.FirstOrDefault(x => x.position == tile.position);
                if (grid != null)
                {
                    grid.exists = true;
                    /*
                    if (tile.isActive)
                    {
                        grid.circle.gameObject.SetActive(true);
                        grid.circleRenderer.material.color = tile.color;
                    } */
                    
                }
                
            }
            
            foreach (var grid in cells)
            {
                if (!grid.exists)
                {
                    grid.delete(levelData);
                }
            }

            int max = levelData.slotData.Count;
            for (int i = 1; i <= max; i++)
            {
                numberOfUfoTrios++;
                editorScale = (float)Mathf.Max(mapSize.x, mapSize.y) / (Mathf.Max(mapSize.x, mapSize.y) + 2);
                SlotData current = levelData.slotData.Find(x => x.orderOfTrio == i);
                UfoTrio newTrio = Instantiate(trioPrefab,
                    transform.position - new Vector3(0f, 0f, (max - i) * editorScale), quaternion.identity);
                newTrio.transform.SetParent(ufoEditor.transform);
                newTrio.Initialize();
                newTrio.transform.localScale *= editorScale;
                var slots = newTrio.GetComponentsInChildren<Grid>();
                foreach (Grid slot in slots)
                {
                    if (slot.name.Equals("Slot 0"))
                    {
                        ufoCol0.Push(slot);
                        slot.orderOfTrio = current.orderOfTrio;
                        slot.circleRenderer.material.color = current.color0;
                    }
                    else if (slot.name.Equals("Slot 1"))
                    {
                        ufoCol1.Push(slot);
                        slot.orderOfTrio = current.orderOfTrio;
                        slot.circleRenderer.material.color = current.color1;


                    }
                    else if (slot.name.Equals("Slot 2"))
                    {
                        ufoCol2.Push(slot);
                        slot.orderOfTrio = current.orderOfTrio;
                        slot.circleRenderer.material.color = current.color2;

                    }
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
            ResetSlots();
            GenerateGrid();
        }

        public void ResetSlots()
        {
            ufoCol0.Clear();
            ufoCol1.Clear();
            ufoCol2.Clear();
            numberOfUfoTrios = 0;
            Debug.Log($"Destroying object: {ufoEditor.gameObject.name}");
            Destroy(ufoEditor.gameObject);
            ufoEditor = new GameObject("Ufo Editor");
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

        private void handleStack()
        {
            if (numberOfUfoTrios > ufoCol0.Count)
            {
                
                moveTriosDown();
                UfoTrio newTrio = Instantiate(trioPrefab, transform.position, quaternion.identity);
                newTrio.Initialize();
                newTrio.transform.SetParent(ufoEditor.transform);
                editorScale = (float)Mathf.Max(mapSize.x, mapSize.y) / (Mathf.Max(mapSize.x, mapSize.y) + 2);
                newTrio.transform.localScale *= editorScale;
                SlotData slotData = new SlotData();
                var slots = newTrio.GetComponentsInChildren<Grid>();
                foreach (Grid slot in slots)
                {
                    if (slot.name.Equals("Slot 0"))
                    {
                        ufoCol0.Push(slot);
                        slot.orderOfTrio = ufoCol0.Count;
                    }
                    else if (slot.name.Equals("Slot 1"))
                    {
                        ufoCol1.Push(slot);
                        slot.orderOfTrio = ufoCol1.Count;

                    }
                    else if (slot.name.Equals("Slot 2"))
                    {
                        ufoCol2.Push(slot);
                        slot.orderOfTrio = ufoCol2.Count;

                    }
                    else
                    {
                        Debug.Log("Something is horribly wrong here");
                    }
                }

                slotData.orderOfTrio = ufoCol0.Count;
                slotData.color0 = ufoCol0.Peek().circleRenderer.material.color;
                slotData.color1 = ufoCol1.Peek().circleRenderer.material.color;
                slotData.color2 = ufoCol2.Peek().circleRenderer.material.color;
                levelData.slotData.Add(slotData);
            }
            else if (numberOfUfoTrios < ufoCol0.Count)
            {
                moveTriosUp();
                levelData.slotData.RemoveAll(x => x.orderOfTrio == ufoCol0.Count);
                Grid grid = ufoCol0.Pop();
                Destroy(grid.gameObject);
                grid = ufoCol1.Pop();
                Destroy(grid.gameObject);
                grid = ufoCol2.Pop();
                Destroy(grid.GetComponentInParent<UfoTrio>().gameObject);
                Destroy(grid.gameObject);
            }
        }

        private void moveTriosDown()
        {
            List<GameObject> children = new List<GameObject>();

            foreach (Transform child in ufoEditor.transform)
            {
                children.Add(child.gameObject);
            }

            foreach (GameObject trio in children)
            {
                trio.transform.position = trio.transform.position - new Vector3(0f, 0f, editorScale);
            }
        }

        private void moveTriosUp()
        {
            List<GameObject> children = new List<GameObject>();

            foreach (Transform child in ufoEditor.transform)
            {
                children.Add(child.gameObject);
            }

            foreach (GameObject trio in children)
            {
                trio.transform.position = trio.transform.position + new Vector3(0f, 0f, editorScale);
            }
        }
    }
}