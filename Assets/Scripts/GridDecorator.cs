using System.Collections.Generic;
using UnityEngine;

public static class GridDecorator
{
#region TETRIS SHAPES
    
    // I
    private static readonly int[,] ShapeI1 = new int[4,4] { 
        { 1, 0, 0, 0 },
        { 1, 0, 0, 0 },
        { 1, 0, 0, 0 },
        { 1, 0, 0, 0 }
     };
     private static readonly int[,] ShapeI2 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 1, 1, 1, 1 }
     };
     
     // T
     private static readonly int[,] ShapeT1 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 1, 1, 1, 0 },
        { 0, 1, 0, 0 }
     };
     private static readonly int[,] ShapeT2 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 1, 0, 0 },
        { 1, 1, 0, 0 },
        { 0, 1, 0, 0 }
     };
     private static readonly int[,] ShapeT3 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 0, 1, 0, 0 },
        { 1, 1, 1, 0 }
     };
     private static readonly int[,] ShapeT4 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 1, 0, 0, 0 },
        { 1, 1, 0, 0 },
        { 1, 0, 0, 0 }
     };
     
     // Z
     private static readonly int[,] ShapeZ1 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 1, 1, 0, 0 },
        { 0, 1, 1, 0 }
     };
     private static readonly int[,] ShapeZ2 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 1, 0, 0 },
        { 1, 1, 0, 0 },
        { 1, 0, 0, 0 }
     };
     
     // S
     private static readonly int[,] ShapeS1 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 0, 1, 1, 0 },
        { 1, 1, 0, 0 }
     };
     private static readonly int[,] ShapeS2 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 1, 0, 0, 0 },
        { 1, 1, 0, 0 },
        { 0, 1, 0, 0 }
     };
     
     // L
     private static readonly int[,] ShapeL1 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 1, 0, 0, 0 },
        { 1, 0, 0, 0 },
        { 1, 1, 0, 0 }
     };
     private static readonly int[,] ShapeL2 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 1, 1, 1, 0 },
        { 1, 0, 0, 0 }
     };
     private static readonly int[,] ShapeL3 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 1, 1, 0, 0 },
        { 0, 1, 0, 0 },
        { 0, 1, 0, 0 }
     };
     private static readonly int[,] ShapeL4 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 0, 0, 1, 0 },
        { 1, 1, 1, 0 }
     };
     
     // J
     private static readonly int[,] ShapeJ1 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 1, 0, 0 },
        { 0, 1, 0, 0 },
        { 1, 1, 0, 0 }
     };
     private static readonly int[,] ShapeJ2 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 1, 0, 0, 0 },
        { 1, 1, 1, 0 }
     };
     private static readonly int[,] ShapeJ3 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 1, 1, 0, 0 },
        { 1, 0, 0, 0 },
        { 1, 0, 0, 0 }
     };
     private static readonly int[,] ShapeJ4 = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 1, 1, 1, 0 },
        { 0, 0, 1, 0 }
     };
     
     // O
     private static readonly int[,] ShapeO = new int[4,4] { 
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 },
        { 1, 1, 0, 0 },
        { 1, 1, 0, 0 }
     };
     
#endregion
    
    private static readonly int[][,] Shapes = new [] { 
        ShapeI1, ShapeI2, 
        ShapeL1, ShapeL2, ShapeL3, ShapeL4, 
        ShapeJ1, ShapeJ2, ShapeJ3, ShapeJ4, 
        ShapeZ1, ShapeZ2, 
        ShapeS1, ShapeS2, 
        ShapeO 
    };
    private static int[] iterateHelper = new [] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    
    public static void DecorateGrid(Grid grid)
    {
        for (int gridCol = 0; gridCol < grid.ColCount; gridCol++)
        {
            for (int gridRow = 0; gridRow < grid.RowCount; gridRow++)
            {
                var currentCell = grid.GetCell(gridCol, gridRow);
                if (currentCell.Item != null)
                {
                    continue;
                }
                
                ShuffleIterateHelper();
                
                for (int shapeIndex = 0; shapeIndex < iterateHelper.Length; shapeIndex++)
                {
                    var shape = Shapes[iterateHelper[shapeIndex]];
                    var shapeCells = new List<Cell>(4);
                    var CanShapeBePlaced = true;
                    
                    for (int shapeCol = 0; shapeCol < 4; shapeCol++)
                    {
                        for (int shapeRow = 0; shapeRow < 4; shapeRow++)
                        {
                            var isExist = shape[3 - shapeRow, shapeCol];
                            if (isExist == 0)
                            {
                                continue;
                            }
                            
                            var cell = grid.GetCell(gridCol + shapeCol, gridRow + shapeRow);
                            if (cell == null || cell.Item != null)
                            {
                                CanShapeBePlaced = false;
                                break;
                            }
                            
                            shapeCells.Add(cell);
                        }
                        
                        if (!CanShapeBePlaced)
                        {
                            break;
                        }
                    }
                    
                    if (CanShapeBePlaced)
                    {
                        var determinedType = DetermineItemTypeForCells(grid, shapeCells);
                        
                        foreach (var cell in shapeCells)
                        {
                            CreateItemForCell(cell, determinedType);
                        }
                        
                        break;
                    }
                }
            }
        }
        
        // Fill Empty Spaces
        for (int gridCol = 0; gridCol < grid.ColCount; gridCol++)
        {
            for (int gridRow = 0; gridRow < grid.RowCount; gridRow++)
            {
                var cell = grid.GetCell(gridCol, gridRow);
                if (cell.Item != null)
                {
                    continue;
                }
                
                var determinedType = DetermineItemTypeForCells(grid, new List<Cell>() { cell });
                CreateItemForCell(cell, determinedType);
            }
        }
    }
    
    private static ItemType DetermineItemTypeForCells(Grid grid, List<Cell> cells)
    {
        var shapeNeighbourItemTypes = new List<ItemType>();
        foreach (var cell in cells)
        {
            // Find Neighbors
            Vector2[] directions = new[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            for (int i = 0; i < directions.Length; i++)
            {
                Vector2 direction = directions[i];
                Cell nCell = grid.GetCell(cell.Col + (int)direction.x, cell.Row + (int)direction.y);
                if (nCell != null && nCell.Item != null)
                {
                    var nType = nCell.Item.ItemType;
                    if (!shapeNeighbourItemTypes.Contains(nType))
                    {
                        shapeNeighbourItemTypes.Add(nType);
                    }
                }
            }
        }
        
        var allTypes = new List<ItemType>() { ItemType.Red, ItemType.Blue, ItemType.Yellow, ItemType.Green, ItemType.Magenta };
        var possibleTypes = new List<ItemType>(allTypes);
        
        foreach (var itemType in shapeNeighbourItemTypes)
        {
            possibleTypes.Remove(itemType);
        }
        
        var determinedTypes = possibleTypes.Count > 0 ? possibleTypes : allTypes;
        return determinedTypes[Random.Range(0, determinedTypes.Count)];
    }
    
    private static void ShuffleIterateHelper()
    {
        int p = iterateHelper.Length;
        for (int n = p - 1; n > 0; n--)
        {
            int r = Random.Range(0, n);
            int t = iterateHelper[r];
            iterateHelper[r] = iterateHelper[n];
            iterateHelper[n] = t;
        }
    }
    
    private static void CreateItemForCell(Cell cell, ItemType itemType)
    {
        var itemGO = GameObject.Instantiate(Game.Instance.itemPrefab);
        itemGO.transform.SetParent(cell.transform, false);
        cell.Item = itemGO.GetComponent<Item>();
        cell.Item.ItemType = itemType;
    }
    
#if UNITY_EDITOR
    public static void ReDecorateGrid(Grid grid)
    {
        for (int col = 0; col < grid.ColCount; col++)
        {
            for (int row = 0; row < grid.RowCount; row++)
            {
                var cell = grid.GetCell(col, row);
                if (cell.Item != null)
                {
                    cell.DestroyItem();
                }
            }
        }
        
        DecorateGrid(grid);
    }
#endif
}