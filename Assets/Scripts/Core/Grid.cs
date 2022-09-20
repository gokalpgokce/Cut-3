using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Scene References")]
    public Transform CellContainer;
    
    private Cell[,] _cells;
    public int RowCount;
    public int ColCount;
    
    public void Init(int colCount, int rowCount)
    {
        _cells = new Cell[colCount, rowCount];
        RowCount = rowCount;
        ColCount = colCount;
        // Create Cells
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                var cellPos = new Vector3(j, i);
                var cellPrefab = Game.Instance.cellPrefab;
                var cellGO = GameObject.Instantiate(cellPrefab, cellPos, Quaternion.identity, CellContainer);
                var cell = cellGO.GetComponent<Cell>();
                cell.Init(j, i);
                _cells[j, i] = cell;
            }
        }
        // Adjust CellContainer Position
        CellContainer.position = new Vector3(-(float)colCount / 2.0f + 0.5f, -(float)rowCount / 2.0f + 0.5f);
    }

    public Cell GetCell(int col,int row)
    {
        if (row < 0 || col < 0 || row > RowCount || col > ColCount)
        {
            return null;
        }
        return _cells[col,row];
    }
    
    public void FindNeihborsOfCell(Cell centerCell)
    {
        List<Cell> neighbors = new List<Cell>();
        neighbors.Add(centerCell);
        GetNeighborsCell(centerCell, neighbors);
        for (var index = 0; index < neighbors.Count; index++)
        {
            var cell = neighbors[index];
            GetNeighborsCell(cell, neighbors);
        }
        foreach (var cell in neighbors)
        {
            Debug.Log("cell of neighbors: " + cell.Col+ "," + cell.Row);
        }
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