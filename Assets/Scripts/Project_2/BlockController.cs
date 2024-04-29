using System.Collections;
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
        public void SetWidth(float blockWidth)
        {
            var scale = transform.localScale;
            transform.localScale = new Vector3(blockWidth, scale.y, scale.z);
        }
        public void Fall()
        {
            StartCoroutine(FallCor());
        }
        private IEnumerator FallCor()
        {
            float timer = 2;
            while (timer > 0)
            {
                timer -= Time.fixedDeltaTime;
                transform.position += Vector3.down * (Time.fixedDeltaTime * 5);
                yield return new WaitForFixedUpdate();
            }

            Destroy(gameObject);
        }
    }
}