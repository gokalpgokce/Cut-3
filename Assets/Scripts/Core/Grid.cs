using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Scene References")]
    public Transform CellContainer;
    
    private Cell[,] _cells;
    public int ColCount;
    public int RowCount;

    public void Init(int colCount, int rowCount)
    {
        _cells = new Cell[colCount, rowCount];
        ColCount = colCount;
        RowCount = rowCount;
        // Create Cells
        for (int i = 0; i < colCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                var cellPos = new Vector3(i, j);
                var cellPrefab = Game.Instance.cellPrefab;
                var cellGO = GameObject.Instantiate(cellPrefab, cellPos, Quaternion.identity, CellContainer);
                var cell = cellGO.GetComponent<Cell>();
                cell.Init(i, j);
                _cells[i, j] = cell;
            }
        }
        // Adjust CellContainer Position
        CellContainer.position = new Vector3(-(float)colCount / 2.0f + 0.5f, -(float)rowCount / 2.0f + 0.5f);
    }

    public Cell GetCell(int col,int row)
    {
        if (col < 0 || row < 0 || col > ColCount-1 || row > RowCount-1)
        {
            return null;
        }
        return _cells[col,row];
    }
    
    // public List<Cell> FindNeighborsOfCell(Cell centerCell)
    // {
    //     List<Cell> neighbors = new List<Cell>();
    //     neighbors.Add(centerCell);
    //     GetNeighborsCell(centerCell, neighbors);
    //     for (var index = 0; index < neighbors.Count; index++)
    //     {
    //         var cell = neighbors[index];
    //         GetNeighborsCell(cell, neighbors);
    //     }
    //     return neighbors;
    // }
    
    public List<Cell> FindCutNeighborsOfCell(Cell centerCell, Cell blockCell)
    {
        List<Cell> cutNeighbors = new List<Cell>();
        cutNeighbors.Add(centerCell);
        GetCutNeighborsCell(centerCell, blockCell, cutNeighbors);
        for (var index = 0; index < cutNeighbors.Count; index++)
        {
            var cell = cutNeighbors[index];
            GetCutNeighborsCell(cell, blockCell,cutNeighbors);
        }
        return cutNeighbors;
    }
    
    // public void GetNeighborsCell(Cell cell, List<Cell> neighbors)
    // {
    //     Vector2[] directions = new[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    //
    //     for (int i = 0; i < directions.Length; i++)
    //     {
    //         Vector2 direction = directions[i];
    //         Cell neighborCell = GetCell(cell.Col+(int)direction.x, cell.Row+(int)direction.y);
    //         if (neighborCell != null && cell.CellType == neighborCell.CellType && !neighbors.Contains(neighborCell))
    //         {
    //             neighbors.Add(neighborCell);
    //         }
    //     }
    // }
    
    public void GetCutNeighborsCell(Cell centerCell, Cell blockCell, List<Cell> neighbors)
    {
        Vector2[] directions = new[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 direction = directions[i];
            Cell cutNeighborCell = GetCell(centerCell.Col+(int)direction.x, centerCell.Row+(int)direction.y);
            if (cutNeighborCell != null && centerCell.Item.ItemType == cutNeighborCell.Item.ItemType && !neighbors.Contains(cutNeighborCell) && cutNeighborCell != blockCell)
            {
                neighbors.Add(cutNeighborCell);
            }
        }
    }

    public Vector3 MousePosToGridPos(Vector3 mousePos)
    {
        Vector3 mouseWorldPos = Game.Instance.cam.ScreenToWorldPoint(mousePos);
        float mouseGamePosX = mouseWorldPos.x + (ColCount/2f);
        float mouseGamePosY = mouseWorldPos.y + (RowCount/2f);
        return new Vector3(mouseGamePosX, mouseGamePosY,0f);
    }

    public List<Cell> FindNearCell(bool isHorizontal, Vector3 position)
    {
        List<Cell> foundCells = new List<Cell>(2);
        if (isHorizontal)
        {
            int topCellRow = Mathf.RoundToInt(position.y);
            int downCellRow = topCellRow - 1;
            int cellCol = (int)position.x;

            Cell topCell = GetCell(cellCol, topCellRow);
            Cell downCell = GetCell(cellCol, downCellRow);
            foundCells.Add(topCell);
            foundCells.Add(downCell);
        }
        else
        {
            int rightCellCol = Mathf.RoundToInt(position.x);
            int leftCellCol = rightCellCol - 1;
            int cellRow = (int)position.y;

            Cell leftCell = GetCell(leftCellCol, cellRow);
            Cell rightCell = GetCell(rightCellCol, cellRow);
            foundCells.Add(leftCell);
            foundCells.Add(rightCell);
        }
        return foundCells;
    }
}