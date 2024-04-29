using UnityEngine;

namespace Grid
{
    public class GridInputController : MonoBehaviour
    {
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            if (!hit.collider.CompareTag("Grid")) return;
            
            GridGameManager.instance.ScreenClicked(hit.collider.GetComponent<GridController>());
        }
    }
}