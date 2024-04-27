using UnityEngine;

namespace Grid
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private GameObject xObject;
        public void PutXOnGrid()
        {
            xObject.SetActive(true);
        }
        public void RemoveXOnGrid()
        {
            xObject.SetActive(false);
        }
    }
}