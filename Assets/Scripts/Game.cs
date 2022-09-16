using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject gridPrefab;
    public GameObject cellPrefab;
    
    private static Game _instance;
    
    private Grid _grid;
    
    void Awake()
    {
        _instance = this;
    }
    
    public static Game Instance
    {
        get { return _instance; }
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
        _grid.Init(9, 9);
    }

    public void PaintCell()
    {
        for (int i = 0; i < _grid.RowCount; i++)
        {
            for (int j = 0; j < _grid.ColCount; j++)
            {
                Cell paintedCell = _grid.GetCell(i,j);
                paintedCell.ChangeCellType(GetRandomCellType());
            }
        }
    }

    public CellType GetRandomCellType()
    {
        CellType[] cellTypes = new[] {CellType.Red, CellType.Blue, CellType.Yellow, CellType.Green, CellType.Magenta};
        int randomResult = Random.Range(0, cellTypes.Length);
        return cellTypes[randomResult];
    }
}
