using System.Collections.Generic;
using UnityEngine;

namespace UfoPuzzle
{
    public class Slot : MonoBehaviour
    {
        public Color color;
        public List<Mesh> meshes;
        public MeshFilter meshFilter;
        public Ufo ufoPrefab;
        public Ufo ufo;
        public Renderer ufoRenderer;

        public void Initialize(SlotData slotData, int num)
        {
            switch (num)
            {
                case 0:
                    color = slotData.color0;
                    transform.position = new Vector3(0f, 0f, -1f);
                    break;
                case 1:
                    color = slotData.color1;
                    transform.position = new Vector3(1f, 0f, -1f);
                    break;
                case 2:
                    color = slotData.color2;
                    transform.position = new Vector3(2f, 0f, -1f);
                    break;
                default:
                    Debug.Log("Something is awfully wrong!");
                    break;
            }
            ufo = Instantiate(ufoPrefab, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            ufo.transform.SetParent(transform);
            ufoRenderer = ufo.ufoRenderer;
            ufoRenderer.material.color = color;
            // meshFilter.mesh = meshes[(position.x + position.y) % meshes.Count];
        }
    }
}