using UnityEngine;

namespace Grid
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private GameObject xObject;
        public bool IsClicked { get; private set; }
        public void PutXOnGrid()
        {
            xObject.SetActive(true);
            IsClicked = true;
        }
        public void RemoveXOnGrid()
        {
            xObject.SetActive(false);
            IsClicked = false;
        }
    }
}