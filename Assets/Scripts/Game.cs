using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [Header("Scene References")]
    public Camera cam;
    public UIController uiController;
    
    [Header("Prefab References")]
    public GameObject gridPrefab;
    public GameObject cellPrefab;
    private Grid _grid;
    
    public const int DefaultRowCount = 12;
    public const int DefaultColCount = 9;
    private int _countNeighbors;

    // Singleton
    private static Game _instance;
    public static Game Instance
    {
        get { return _instance; }
    }
    
    void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (!uiController.mainUIGO.activeSelf)
        {
            SelectCell();
        }
    }

    public void PlayGame()
    {
        InitGame();
    }
    
    public void InitGame()
    {
        CreateGrid();
        PaintCell();
    }
    
    private void CreateGrid()
    {
        // Instantiate grid prefab under this (Game) gameobject
        var gridGO = GameObject.Instantiate(gridPrefab, Vector3.zero, Quaternion.identity, transform);
        _grid = gridGO.GetComponent<Grid>();
        _grid.Init(DefaultColCount, DefaultRowCount);
    }

    public void PaintCell()
    {
        for (int i = 0; i < _grid.ColCount; i++)
        {
            for (int j = 0; j < _grid.RowCount; j++)
            {
                Cell paintedCell = _grid.GetCell(i,j);
                paintedCell.CellType = GetRandomCellType();
            }
        }
    }

    public CellType GetRandomCellType()
    {
        CellType[] cellTypes = new[] {CellType.Red, CellType.Blue, CellType.Yellow, CellType.Green, CellType.Magenta};
        int randomResult = Random.Range(0, cellTypes.Length);
        return cellTypes[randomResult];
    }

    public void SelectCell()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseGridPos = _grid.MousePosToGridPos(Input.mousePosition);
            if (mouseGridPos.x >= 0 && mouseGridPos.y >= 0 && mouseGridPos.x < DefaultColCount && mouseGridPos.y < DefaultRowCount)
            {
                Cell testCell = _grid.GetCell((int) mouseGridPos.x, (int) mouseGridPos.y);
                List<Cell> testList = _grid.FindNeihborsOfCell(testCell);
                // foreach (var cell in testList)
                // {
                //     Debug.Log("cell of neighbors: " + cell);
                // }
            }
        }
    }

    public void OnSwipe(Vector3 swipeStartMouse, Vector3 swipeEndMouse)
    {
        Vector3 swipeStart = _grid.MousePosToGridPos(swipeStartMouse);
        Vector3 swipeEnd = _grid.MousePosToGridPos(swipeEndMouse);
        bool isHorizontal = Mathf.Abs(swipeStart.x - swipeEnd.x) > Mathf.Abs(swipeStart.y - swipeEnd.y);
        Vector3 swipeCenter = (swipeStart + swipeEnd) / 2.0f;
        List<Cell> foundedCells = _grid.FindNearCell(isHorizontal, swipeCenter);
        CheckValidCut(foundedCells[0],foundedCells[1]);
    }

    public bool CheckValidCut(Cell cell1, Cell cell2)
    {
        if (cell1 == null || cell2 == null)
        {
            return false;
        }

        if (cell1.CellType != cell2.CellType)
        {
            return false;
        }
        // TODO: find neighbors cell1 cell2
        return true;
    }
}
