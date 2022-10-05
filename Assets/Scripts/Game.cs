using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [Header("Scene References")]
    public Camera cam;
    public UIController uiController;
    public AudioManager audioManager;
    public ObjectPooler itemPooler;
    public ObjectPooler particlePooler;
    public ObjectPooler cellPooler;
    
    [Header("Prefab References")]
    public GameObject gridPrefab;
    public GameObject cellPrefab;
    public GameObject itemPrefab;
    private Grid _grid;
    private GameState _gameState = GameState.NotStarted;
    public int fallCounter = 0;
    
    public const int DefaultRowCount = 12;
    public const int DefaultColCount = 9;
    public const float SwipeThreshold = 1.0f;

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
        itemPooler.WarmUp();
        cellPooler.WarmUp();
        particlePooler.WarmUp();
        InitGame();
        StartCoroutine(GameRoutine());
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
    }

    IEnumerator GameRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            _gameState = fallCounter > 0 ? GameState.Fall : GameState.WaitingForInput;
            
            if (fallCounter > 0)
            {
                while (fallCounter > 0)
                {
                    yield return null;
                }
                CheckThreeItems();
            }
            yield return null;
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
                        GameObject itemGO = itemPooler.Get();
                        itemGO.transform.position = topCellPos + (Vector3.up * counter);
                        itemGO.transform.rotation = Quaternion.identity;
                        itemGO.transform.parent = spawnCell.transform;
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
        if (_gameState == GameState.WaitingForInput)
        {
            Vector3 swipeStart = _grid.MousePosToGridPos(swipeStartMouse);
            Vector3 swipeEnd = _grid.MousePosToGridPos(swipeEndMouse);
            if ((swipeEnd-swipeStart).magnitude > SwipeThreshold)
            {
                bool isHorizontal = Mathf.Abs(swipeStart.x - swipeEnd.x) > Mathf.Abs(swipeStart.y - swipeEnd.y);
                Vector3 swipeCenter = (swipeStart + swipeEnd) / 2.0f;
                List<Cell> foundedCells = _grid.FindNearCell(isHorizontal, swipeCenter);
                CheckValidCut(foundedCells[0],foundedCells[1]); 
            }
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
            audioManager.PlaySwipeSound();
            DestroyCells(cutLeftUpNeighborsOfCell);
            isDestroy = true;
        }
        if (cutRightDownNeighborsOfCell.Count == 3)
        {
            audioManager.PlaySwipeSound();
            DestroyCells(cutRightDownNeighborsOfCell);
            isDestroy = true;
        }

        if (isDestroy)
        {
            ExecuteAfterDestroy();
        }
        
        return true;
    }

    public void CheckThreeItems()
    {
        bool isDestroy = false;
        for (int i = 0; i < _grid.ColCount; i++)
        {
            for (int j = 0; j < _grid.RowCount; j++)
            {
                Cell cell = _grid.GetCell(i, j);
                List<Cell> threeNeighbors = _grid.FindCutNeighborsOfCell(cell, null);
                if (threeNeighbors.Count == 3)
                {
                    isDestroy = true;
                    DestroyCells(threeNeighbors);
                }
            }
        }
        if (isDestroy)
        {
            ExecuteAfterDestroy();
        }
    }

    public void ExecuteAfterDestroy()
    {
        _grid.Fall();
        CreateItemsForTop();
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
