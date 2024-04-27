using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GridGameManager : MonoBehaviour
{
    public static GridGameManager instance;
    public event Action<int> MatchCountChange;
    [SerializeField] private Transform gridHolder;
    [SerializeField] private GridController gridPrefab;
    [SerializeField] private int startGridCount = 3;
    [SerializeField] private float layoutSize = 12;
    [SerializeField] private float gridSizeMultiplier = 12;
    private readonly List<GridController> _createdGrids = new();
    private int _matchCount;
    private int _gridCount;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetNewGridCount(startGridCount);
    }
    public void SetNewGridCount(int newGridCount)
    {
        _gridCount = newGridCount;
        var gridSize = layoutSize / newGridCount;

        _matchCount = 0;
        MatchCountChange?.Invoke(_matchCount);

        foreach (var gridController in _createdGrids)
            Destroy(gridController.gameObject);
        _createdGrids.Clear();
        for (var x = 0; x < _gridCount; x++)
        {
            for (var y = 0; y < _gridCount; y++)
            {
                var cellPosition = new Vector3(x * gridSize, -y * gridSize, 0f);
                cellPosition += new Vector3(.5f * gridSize, -.5f * gridSize, 0);
                var createdGrid = Instantiate(gridPrefab, gridHolder);
                createdGrid.transform.localPosition = cellPosition;
                createdGrid.transform.localScale = gridSizeMultiplier * gridSize * Vector3.one;
                _createdGrids.Add(createdGrid);
            }
        }
    }
#if UNITY_EDITOR
    [Header("Editor")] [SerializeField] private int editorGridCount = 3;
    [ContextMenu("SetGridCount")]
    public void SetGridCountEditor()
    {
        SetNewGridCount(editorGridCount);
    }
#endif
}