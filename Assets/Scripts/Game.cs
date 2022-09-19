using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Scene References")]
    public Camera cam;
    public UIController uiController;
    
    [Header("Prefab References")]
    public GameObject gridPrefab;
    public GameObject cellPrefab;
    
    public const int DefaultRowCount = 12;
    public const int DefaultColCount = 9;
    
    private int _countNeighbors;
    
    private Grid _grid;
    
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
    
    void Start()
    {
        CalculateOrthographicSize();
    }
    
    public void PlayGame()
    {
        InitGame();
    }
    
    public void InitGame()
    {
        CreateGrid();
        PaintCell();
        FindNeighbors();
    }
    
    private void CreateGrid()
    {
        // Instantiate grid prefab under this (Game) gameobject
        var gridGO = GameObject.Instantiate(gridPrefab, Vector3.zero, Quaternion.identity, transform);
        _grid = gridGO.GetComponent<Grid>();
        _grid.Init(DefaultRowCount, DefaultColCount);
    }

    public void PaintCell()
    {
        for (int i = 0; i < _grid.RowCount; i++)
        {
            for (int j = 0; j < _grid.ColCount; j++)
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

    public void FindNeighbors()
    {
        Cell centerCell = _grid.GetCell(2, 2);
        CellType centerCellType = centerCell.CellType;
        List<Cell> matchCells = new List<Cell>();
        List<Cell> neighborCells = GetNeighborsCell(centerCell.Row,centerCell.Col);
        
        for (int i = 0; i < neighborCells.Count; i++)
        {
            CellType neighborCellType = neighborCells[i].CellType;
            if (centerCellType == neighborCellType)
            {
                matchCells.Add(neighborCells[i]);
            }
        }
    }
    
    public List<Cell> GetNeighborsCell(int row,int col)
    {
        List<Cell> neighborsCells = new List<Cell>();
        Vector2[] directions = new[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 direction = directions[i];
            Cell neighborCell = _grid.GetCell(row+(int)direction.x, col+(int)direction.y);
            neighborsCells.Add(neighborCell);
        }
        return neighborsCells;
    }

    private void CalculateOrthographicSize()
    {
        float size = (Screen.height / 2.0f * 5.0f) / (Screen.width / 2.0f);
        size = Mathf.Max(size, DefaultRowCount / 2.0f + 1.0f);
        cam.orthographicSize = size;
    }
    
#if true
    void OnGUI()
    {
        if (GUI.Button(new Rect(25, 25, 150, 100), "Calcuate"))
        {
            CalculateOrthographicSize();
            _grid.CellContainer.position = new Vector3(-(float)_grid.ColCount / 2.0f + 0.5f, -(float)_grid.RowCount / 2.0f + 0.5f);
        }
    }
#endif
}
