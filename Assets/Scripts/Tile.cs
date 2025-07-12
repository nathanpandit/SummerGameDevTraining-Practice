using System.Collections.Generic;
using UnityEngine;

namespace UfoPuzzle
{
    public class Tile : MonoBehaviour
    {
        public Vector2Int position;
        public Color color;
        public List<Mesh> meshes;
        public MeshFilter meshFilter;
        public Circle circlePrefab;
        public Circle circle;
        public Renderer circleRenderer;
        public bool isVisited;

        public void Initialize(TileData tileData)
        {
            position = tileData.position;
            transform.position = new Vector3(position.x, 0f, position.y);
            circle = Instantiate(circlePrefab, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            circle.transform.SetParent(transform);
            circleRenderer = circle.circleRenderer;
            isVisited = false;
            // meshFilter.mesh = meshes[(position.x + position.y) % meshes.Count];
        }
    }
}