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
        private GameObject slotParent;
        private int ufoCount;
        public Dictionary<Transform, bool> spawnSlots = new Dictionary<Transform, bool>();

        public void Initialize(List<UfoData> _ufoData)
        {
            ufoCount = 0;
            if(slotParent != null) Destroy(slotParent);
            slotParent = new GameObject("Slots");
            spawnPoints.Clear();
            UfoData = _ufoData;
            
            Ray ray0 = Camera.main.ScreenPointToRay(new Vector3(Screen.width * .4f, Screen.height * .15f, 0));
            Ray ray1 = Camera.main.ScreenPointToRay(new Vector3(Screen.width * .5f, Screen.height * .15f, 0));
            Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(Screen.width * .6f, Screen.height * .15f, 0));
            
            Plane plane = new Plane(Vector3.up, Vector3.up/2.0f);

            if (plane.Raycast(ray0, out float distance))
            {
                Vector3 targetPosition = ray0.GetPoint(distance);
                Slot slot = Instantiate(slotPrefab, targetPosition, quaternion.identity);
                slot.gameObject.name = "Slot 0";
                slot.gameObject.transform.SetParent(slotParent.transform);
                spawnPoints.Add(slot.gameObject.transform);
                spawnPoints[0].position = targetPosition;
                ;
            }
            if (plane.Raycast(ray1, out distance))
            {
                Vector3 targetPosition = ray1.GetPoint(distance);
                Slot slot = Instantiate(slotPrefab, targetPosition, quaternion.identity);
                slot.gameObject.name = "Slot 1";
                slot.gameObject.transform.SetParent(slotParent.transform);
                spawnPoints.Add(slot.gameObject.transform);
                spawnPoints[1].position = targetPosition;
                
            }
            if (plane.Raycast(ray2, out distance))
            {
                Vector3 targetPosition = ray2.GetPoint(distance);
                Slot slot = Instantiate(slotPrefab, targetPosition, quaternion.identity);
                slot.gameObject.name = "Slot 2";
                slot.gameObject.transform.SetParent(slotParent.transform);
                spawnPoints.Add(slot.gameObject.transform);
                spawnPoints[2].position = targetPosition;
            }
            
            for (int i = 0; i < 3; i++)
            {
                Debug.Log("7");

                spawnSlots.Add(spawnPoints[i], true);
                Debug.Log("8");

            }
            
            if(ufoParent != null) Destroy(ufoParent.gameObject);
            
            ufoParent = new GameObject("Ufos").transform;
            SpawnInitialBlocks();
        }
        
        public void SpawnInitialBlocks()
        {
            for (int i = 0; i < 3; i++)
            {
                spawnSlots[spawnPoints[i]] = true;
                SpawnUfo();
            }
        }

        private void SpawnUfo()
        {
            Debug.Log($"Current ufo count is {ufoCount}");
            UfoData ufoData = UfoData[ufoCount];
            Debug.Log(ufoData.color);
            Ufo newUfo = Instantiate(ufoPrefab, spawnPoints[ufoCount%3].position, quaternion.identity);
            newUfo.gameObject.name = $"Ufo {ufoCount}";
            newUfo.transform.SetParent(ufoParent);
            newUfo.ufoRenderer.material.color = ufoData.color;
            newUfo.originalPos = newUfo.transform.position;
            ufoCount++;
        }
    }
}