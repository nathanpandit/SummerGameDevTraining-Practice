using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WoodPuzzle
{
    public class Cube : MonoBehaviour
    {
        [Header("Cube Properties")]
        public Vector2Int position;
        public ObjectColor color;
        
        [Header("Visual Components")]
        public List<Mesh> meshes;
        public MeshRenderer meshRenderer;
        public MeshFilter meshFilter;
        
        public void Initialize(CubeData cubeData)
        {
            position = cubeData.position;
            color = (ObjectColor)(((int)cubeData.objectColor));
            SetupTransform();
        }
        
        
        private void SetupTransform()
        {
            transform.position = new Vector3(position.x, 0f, position.y);
        }
        
        
        
        /*public void Destroy()
        {
            // Update wood objects of neighboring cubes
            Vector2Int[] directions = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down,
                Vector2Int.left
            };

            for (int i = 0; i < directions.Length; i++)
            {
                Vector2Int neighborPos = position + directions[i];
                var neighborCube = block.cubes.FirstOrDefault(c => c.position == neighborPos);
                
                if (neighborCube != null)
                {
                    // The opposite direction index is used because we're updating the neighbor's wood object
                    // that was connected to this cube
                    int oppositeIndex = (i + 2) % 4;
                    neighborCube.SetWoodObjectsActive(oppositeIndex, true);
                }
            }

            // If this cube has a key, decrease the counter of locked blocks
            if (hasKey)
            {
                DragHelper.DecreaseLockedCounter();
            }

            block.isInExitContact = false;
            block.exitInContact = null;
            Destroy(gameObject);
            block.RemoveCube(this);
        } */
        
    }
}
