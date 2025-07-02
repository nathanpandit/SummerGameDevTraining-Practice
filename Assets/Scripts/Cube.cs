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
        public Block block;
        public bool hasKey;
        public bool hasIce;
        public ObjectMaterial material;
        
        [Header("Visual Components")]
        public List<Material> materials;
        public GameObject starObject;
        public List<Mesh> meshes;
        public Material iceMaterial;
        public MeshRenderer meshRenderer;
        public MeshFilter meshFilter;
        public Outline outline;
        public ParticleSystem iceParticleSystem;
        public GameObject iceReflect;
        public GameObject keyObject;
        
        public void Initialize(CubeData cubeData, Block _block)
        {
            block = _block;
            position = cubeData.position;
            color = (ObjectColor)(((int)cubeData.objectColor + LevelHelper.GetLoopCount()) % 8);
            hasKey = cubeData.hasKey;
            material = cubeData.material;
            hasIce = block.movement == Movement.Ice;
            
            keyObject.SetActive(hasKey);
            transform.SetParent(_block.transform);
            SetupTransform();
            SetupVisuals();
            AddAutline();
        }
        
        public void SetWoodObjectsActive(int index, bool isActive)
        {
            if (index < 0 || index >= 4)
            {
                Debug.LogError($"Index {index} is out of range for woodObjects list.");
                return;
            }
            
            var scale = meshRenderer.transform.localScale;
            var pos = meshRenderer.transform.localPosition;
            
            Vector2Int[] directions = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down,
                Vector2Int.left
            };

            Vector2Int neighborPos = position + 2 * directions[index];
            var neighborCube = block.cubes.FirstOrDefault(c => c.position == neighborPos);
            if (neighborCube != null)
            {
                Debug.Log($"Cube at {position} has a neighbor at {neighborPos}");
                if (index == 0)
                {
                    pos.z = -0.075f;
                }
                else if (index == 1)
                {
                    pos.x = -0.075f;
                }
                else if (index == 2)
                {
                    pos.z = 0.075f;
                }
                else if (index == 3)
                {
                    pos.x = 0.075f;
                }
            }
            else
            {
                if (index % 2 == 0)
                {
                    scale.y = 0.67f;
                    pos.z = 0;
                }
                else
                {
                    scale.x = 0.67f;
                    pos.x = 0;
                }
            }
            
            meshRenderer.transform.localScale = scale;
            meshRenderer.transform.localPosition = pos;
        }
        
        private void SetupTransform()
        {
            transform.position = new Vector3(position.x, 0f, position.y);
        }
        
        private void SetupVisuals()
        {
            meshFilter.mesh = meshes[(position.x + position.y) % meshes.Count];
            if (hasIce)
            {
                iceReflect.SetActive(true);
                meshRenderer.material = iceMaterial;
            }
            else
            {
                if (material == ObjectMaterial.Metal)
                {
                    starObject.SetActive(true);
                }
                meshRenderer.material = materials[((int)color)];
            }
        }
        
        public void ChangeIceMaterial()
        {
            if (!hasIce) return;
            
            if (material == ObjectMaterial.Metal)
            {
                starObject.SetActive(true);
            }
            meshRenderer.material = materials[((int)color)];
            
            iceParticleSystem.Play();
            iceReflect.SetActive(false);
            hasIce = false;
        }
        
        public void Destroy()
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
        }
        
        private void AddAutline()
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 5f;
            outline.enabled = false;
        }
        
        public void SetOutline(bool isActive, Color? color = null)
        {
            outline.enabled = isActive;
            if (color.HasValue)
            {
                outline.OutlineColor = color.Value;
            }
        }
    }
}
