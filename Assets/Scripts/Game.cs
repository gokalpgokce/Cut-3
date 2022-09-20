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
    private Grid _grid;
    
    private const int DefaultRowCount = 12;
    private const int DefaultColCount = 9;
    private int _countNeighbors;

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

    private void Update()
    {
        FindMousePos();
    }

    public void PlayGame()
    {
        InitGame();
    }
    
    public void InitGame()
    {
        CreateGrid();
        PaintCell();
        _grid.FindNeihborsOfCell(_grid.GetCell(2,2));
    }
    
    private void CreateGrid()
    {
        // Instantiate grid prefab under this (Game) gameobject
        var gridGO = GameObject.Instantiate(gridPrefab, Vector3.zero, Quaternion.identity, transform);
        _grid = gridGO.GetComponent<Grid>();
        _grid.Init(DefaultColCount, DefaultRowCount);
    }

    public void PaintCell()
    {
        for (int i = 0; i < _grid.RowCount; i++)
        {
            for (int j = 0; j < _grid.ColCount; j++)
            {
                Cell paintedCell = _grid.GetCell(j,i);
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

    public void FindMousePos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float mouseGamePosX = mouseWorldPos.x + (DefaultColCount/2f);
            float mouseGamePosY = mouseWorldPos.y + (DefaultRowCount/2f);
            if (mouseGamePosX >= 0 && mouseGamePosY >= 0 && mouseGamePosX < DefaultColCount && mouseGamePosY < DefaultRowCount)
            {
                Debug.Log("mouse world pos: " + mouseWorldPos);
                Debug.Log("selected cell: " +(int)mouseGamePosX + ", " + (int)mouseGamePosY);
            }
        }
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
