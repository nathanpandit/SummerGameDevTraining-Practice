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

        GameObject cubePrefab;
        GameObject exitPrefab;

        public GameObject cube;
        GameObject exit;

        private void Awake()
        {
            cubePrefab = LevelEditorA.Instance().cubePrefab;
            exitPrefab = LevelEditorA.Instance().exitPrefab;
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

        public void addCube(CubeData cubeData, Grid referenceGrid, BlockData blockdata)
        {
            removeCube();

            cube = Instantiate(cubePrefab, referenceGrid.transform);
            cube.transform.position = new Vector3 (cubeData.position.x, 0.87f, cubeData.position.y);

            ObjectColor colorKey;
            Enum.TryParse(cubeData.objectColor.ToString().Replace("Metal", ""), out colorKey);

            Color cubeColor = LevelEditorA.Instance().colorDict[colorKey];
            cube.GetComponent<Renderer>().material.color = cubeColor;

            Movement movement = blockdata.movement;
            int counter = blockdata.counter;


            string blockIndicator = $"{referenceGrid.position.x} {referenceGrid.position.y}" ;

            if (cubeData.material == ObjectMaterial.Metal)
            {
                blockIndicator = "[" + blockIndicator + "]";
            }

            switch (movement)
            {
                case Movement.Free:
                    {
                        break;
                    }
                case Movement.Horizontal:
                    {
                        blockIndicator += $"\n-";
                        break;
                    }
                case Movement.Vertical:
                    {
                        blockIndicator = $"\n|";
                        break;
                    }
                case Movement.Ice:
                    {
                        blockIndicator += $"\nB{counter}";
                        break;
                    }
                case Movement.Locked:
                    {
                        blockIndicator = $"\nK{counter}";

                        break;
                    }
            }

            if (cubeData.hasKey)
            {
                blockIndicator = blockIndicator.Replace(" ", "o");

            }

                TextMeshPro indicatorTMP = cube.transform.GetChild(0).GetComponent<TextMeshPro>();
            indicatorTMP.SetText(blockIndicator);
        }

        public void removeCube()
        {
            if(cube != null)
            {
                Destroy(cube);
                cube = null;
            }
        }

        public void addExit(ExitData exitData, Grid referenceGrid)
        {
            removeExit();
            
            exit = Instantiate(exitPrefab, parent: referenceGrid.transform);
            exit.transform.position = new Vector3(exitData.position.x, 0.87f, exitData.position.y);

            ObjectColor colorKey;
            Enum.TryParse(exitData.exitColor.ToString().Replace("Metal", ""), out colorKey);

            foreach (Transform part in exit.transform)
            {
                part.GetComponent<Renderer>().material.color = LevelEditorA.Instance().colorDict[colorKey];
            }

            float degree = ((int)exitData.direction * 90);

            exit.transform.rotation = Quaternion.Euler(new Vector3(0, degree));

            TextMeshPro indicator = exit.transform.GetChild(0).GetComponent<TextMeshPro>();

            if (exitData.material == ObjectMaterial.Metal)
            {
                indicator.SetText("M");
            }
        }

        public void removeExit()
        {
            if (exit)
            {
                Destroy(exit);
                exit = null;
            }
        }
    
        /*
        public void addObstacle()
        {
            gridRenderer.material.color = LevelEditorA.Instance().obstacleGridColor;
        }
        */

        public void removeObstacle()
        {
            setTile();
        }
    }

}