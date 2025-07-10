using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace UfoPuzzle
{
    public class UfoSpawner : MonoBehaviour
    {
        private List<UfoData> UfoData;
        public Ufo ufoPrefab;
        public Slot slotPrefab;
        public List<Transform> spawnPoints = new List<Transform>(3);
        private Transform ufoParent;
        private int ufoCount = 0;
        public Dictionary<Transform, bool> spawnSlots = new Dictionary<Transform, bool>();

        public void Initialize(List<UfoData> _ufoData)
        {
            UfoData = _ufoData;
            
            Ray ray0 = Camera.main.ScreenPointToRay(new Vector3(Screen.width * .4f, Screen.height * .15f, 0));
            Ray ray1 = Camera.main.ScreenPointToRay(new Vector3(Screen.width * .5f, Screen.height * .15f, 0));
            Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(Screen.width * .6f, Screen.height * .15f, 0));
            
            Plane plane = new Plane(Vector3.up, Vector3.up/2.0f);

            if (plane.Raycast(ray0, out float distance))
            {
                Vector3 targetPosition = ray0.GetPoint(distance);
                Debug.Log("1");
                spawnPoints.Add(new GameObject("Slot 0").transform);
                spawnPoints[0].position = targetPosition;
                Debug.Log("2");
                Slot slot = Instantiate(slotPrefab, targetPosition, quaternion.identity);
            }
            if (plane.Raycast(ray1, out distance))
            {
                Vector3 targetPosition = ray1.GetPoint(distance);
                Debug.Log("3");
                spawnPoints.Add(new GameObject("Slot 1").transform);
                spawnPoints[1].position = targetPosition;
                Debug.Log("4");

                Slot slot = Instantiate(slotPrefab, targetPosition, quaternion.identity);

            }
            if (plane.Raycast(ray2, out distance))
            {
                Vector3 targetPosition = ray2.GetPoint(distance);
                Debug.Log("5");
                spawnPoints.Add(new GameObject("Slot 2").transform);
                spawnPoints[2].position = targetPosition;
                Debug.Log("6");

                Slot slot = Instantiate(slotPrefab, targetPosition, quaternion.identity);
            }
            
            for (int i = 0; i < 3; i++)
            {
                Debug.Log("7");

                spawnSlots.Add(spawnPoints[i], true);
                Debug.Log("8");

            }
            
            ufoParent = new GameObject("Ufos").transform;
            SpawnInitialBlocks();
        }
        
        public void SpawnInitialBlocks()
        {
            for (int i = 0; i < 3; i++)
            {
                Debug.Log("9");

                spawnSlots[spawnPoints[i]] = true;
                Debug.Log("10");

                SpawnUfo();
            }
        }

        private void SpawnUfo()
        {
            Debug.Log("11");

            UfoData ufoData = UfoData[ufoCount];
            Debug.Log("12");


            Ufo newUfo = Instantiate(ufoPrefab, spawnPoints[ufoCount%3].position, quaternion.identity);
            Debug.Log("13");

            newUfo.gameObject.name = $"Ufo {ufoCount}";
            newUfo.transform.SetParent(ufoParent);
            newUfo.ufoRenderer.material.color = ufoData.color;
            ufoCount++;
        }
    }
}