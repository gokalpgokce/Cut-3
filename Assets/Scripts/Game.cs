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
    }
    
    private void CreateGrid()
    {
        // Instantiate grid prefab under this (Game) gameobject
        var gridGO = GameObject.Instantiate(gridPrefab, Vector3.zero, Quaternion.identity, transform);
        _grid = gridGO.GetComponent<Grid>();
        _grid.Init(9, 9);
    }
}
