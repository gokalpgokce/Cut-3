using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Scene References")]
    public Transform CellContainer;
    
    public Cell[,] _cells;
    public int RowCount;
    public int ColCount;
    
    
    public void Init(int rowCount, int colCount)
    {
        _cells = new Cell[rowCount, colCount];
        RowCount = rowCount;
        ColCount = colCount;
        
        // Create Cells
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
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
        CellContainer.position = new Vector3(-rowCount / 2.0f, -colCount / 2.0f);
    }
}