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
    
    public List<Cell> FindNeihborsOfCell(Cell centerCell)
    {
        List<Cell> neighbors = new List<Cell>();
        neighbors.Add(centerCell);
        GetNeighborsCell(centerCell, neighbors);
        for (var index = 0; index < neighbors.Count; index++)
        {
            var cell = neighbors[index];
            GetNeighborsCell(cell, neighbors);
        }
        return neighbors;
    }
    
    public void GetNeighborsCell(Cell cell, List<Cell> neighbors)
    {
        Vector2[] directions = new[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 direction = directions[i];
            Cell neighborCell = GetCell(cell.Col+(int)direction.x, cell.Row+(int)direction.y);
            if (neighborCell != null && cell.CellType == neighborCell.CellType && !neighbors.Contains(neighborCell))
            {
                neighbors.Add(neighborCell);
            }
        }
    }
}