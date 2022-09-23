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

    public void PlayGame()
    {
        InitGame();
        isGameStarted = true;
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
        GridDecorator.DecorateGrid(_grid);
        // for (int i = 0; i < _grid.ColCount; i++)
        // {
        //     for (int j = 0; j < _grid.RowCount; j++)
        //     {
        //         Cell cell = _grid.GetCell(i,j);
                
        //         GameObject itemGO = GameObject.Instantiate(itemPrefab,cell.transform);
        //         cell.Item = itemGO.GetComponent<Item>();
        //         cell.Item.ItemType = GetRandomItemType();
        //     }
        // }
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

        if (cutLeftUpNeighborsOfCell.Count == 3)
        {
            DestroyCells(cutLeftUpNeighborsOfCell);
        }
        if (cutRightDownNeighborsOfCell.Count == 3)
        {
            DestroyCells(cutRightDownNeighborsOfCell);
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
    
#if UNITY_EDITOR
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "Re-Decorate"))
        {
            GridDecorator.ReDecorateGrid(_grid);
        }
    }
#endif
}
