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
    public GameObject itemPrefab;
    private Grid _grid;
    private bool isGameStarted = false;
    
    public const int DefaultRowCount = 12;
    public const int DefaultColCount = 9;

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

    private void IsGameStarted()
    {
        isGameStarted = true;
    }

    public void PlayGame()
    {
        InitGame();
        Invoke(nameof(IsGameStarted),1);
    }
    
    public void InitGame()
    {
        CreateGrid();
        CreateItems();
    }

    private void CreateGrid()
    {
        // Instantiate grid prefab under this (Game) gameobject
        var gridGO = GameObject.Instantiate(gridPrefab, Vector3.zero, Quaternion.identity, transform);
        _grid = gridGO.GetComponent<Grid>();
        _grid.Init(DefaultColCount, DefaultRowCount);
    }

    public void CreateItems()
    {
        for (int i = 0; i < _grid.ColCount; i++)
        {
            for (int j = 0; j < _grid.RowCount; j++)
            {
                Cell cell = _grid.GetCell(i,j);
                
                GameObject itemGO = GameObject.Instantiate(itemPrefab,cell.transform);
                cell.Item = itemGO.GetComponent<Item>();
                cell.Item.ItemType = GetRandomItemType();
            }
        }
    }

    public void CreateItemsForTop()
    {
        for (int i = 0; i < _grid.ColCount; i++)
        {
            for (int j = 0; j < _grid.RowCount; j++)
            {
                Cell cell = _grid.GetCell(i, j);
                if (cell.Item == null)
                {
                    Cell topCell = _grid.GetCell(cell.Col, _grid.RowCount-1);
                    Vector3 topCellPos = topCell.transform.position;
                    int counter = 1;
                    for (int k = cell.Row; k < _grid.RowCount; k++)
                    {
                        Cell spawnCell = _grid.GetCell(i, k);
                        GameObject itemGO = Instantiate(itemPrefab, topCellPos + (Vector3.up * counter), Quaternion.identity,
                            spawnCell.transform);
                        spawnCell.Item = itemGO.GetComponent<Item>();
                        spawnCell.Item.ItemType = GetRandomItemType();
                        spawnCell.SpawnItem();
                        counter++;
                    }
                }
            }
        }
    }

    public ItemType GetRandomItemType()
    {
        ItemType[] itemTypes = new[] {ItemType.Red, ItemType.Blue, ItemType.Yellow, ItemType.Green};
        int randomResult = Random.Range(0, itemTypes.Length);
        return itemTypes[randomResult];
    }
    
    public void OnSwipe(Vector3 swipeStartMouse, Vector3 swipeEndMouse)
    {
        if (isGameStarted)
        {
            Vector3 swipeStart = _grid.MousePosToGridPos(swipeStartMouse);
            Vector3 swipeEnd = _grid.MousePosToGridPos(swipeEndMouse);
            bool isHorizontal = Mathf.Abs(swipeStart.x - swipeEnd.x) > Mathf.Abs(swipeStart.y - swipeEnd.y);
            Vector3 swipeCenter = (swipeStart + swipeEnd) / 2.0f;
            List<Cell> foundedCells = _grid.FindNearCell(isHorizontal, swipeCenter);
            CheckValidCut(foundedCells[0],foundedCells[1]); 
        }
    }

    public bool CheckValidCut(Cell cell1, Cell cell2)
    {
        if (cell1 == null || cell2 == null)
        {
            return false;
        }

        if (!cell1.IsSameType(cell2))
        {
            return false;
        }

        List<Cell> cutLeftUpNeighborsOfCell = _grid.FindCutNeighborsOfCell(cell1,cell2);
        List<Cell> cutRightDownNeighborsOfCell = _grid.FindCutNeighborsOfCell(cell2,cell1);

        bool isDestroy = false;
        if (cutLeftUpNeighborsOfCell.Count == 3)
        {
            DestroyCells(cutLeftUpNeighborsOfCell);
            isDestroy = true;
        }
        if (cutRightDownNeighborsOfCell.Count == 3)
        {
            DestroyCells(cutRightDownNeighborsOfCell);
            isDestroy = true;
        }

        if (isDestroy)
        {
           _grid.FindEmptyCell();
            CreateItemsForTop();
        }
        
        return true;
    }

    public void DestroyCells(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            cell.DestroyItem();
        }
    }
}
