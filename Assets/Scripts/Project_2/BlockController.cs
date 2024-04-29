using UnityEngine;

namespace Project_2
{
    public class BlockController : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
        public void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }
    }
}