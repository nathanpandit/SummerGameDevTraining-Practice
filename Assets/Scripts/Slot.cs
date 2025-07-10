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

    }
}