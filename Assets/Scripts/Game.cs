using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public ObjectPooler specialPooler;
    
    [Header("Prefab References")]
    public GameObject gridPrefab;
    public GameObject cellPrefab;
    public GameObject itemPrefab;
    private Grid _grid;
    private GameState _gameState = GameState.NotStarted;
    private Coroutine gameCoroutine;
    public ParticleSystem trailParticle;
    public ParticleSystem winParticle;
    public ItemType selectedType;
    public int specialItemsCount;
    public int specialItemsTotal;
    public int fallCounter = 0;
    private int _score;
    public int booster = 3;
    public int colorChange = 2;
    
    public const int DefaultRowCount = 12;
    public const int DefaultColCount = 9;
    public const float SwipeThreshold = 1.0f;

    // Singleton
    private static Game _instance;
    public static Game Instance
    {
        get { return _instance; }
    }

    public GameState GameState
    {
        get { return _gameState; }
        set { _gameState = value; }
    }
    
    void Awake()
    {
        _instance = this;
    }

    public void WarmUpPools()
    {
        cellPooler.WarmUp();
        itemPooler.WarmUp();
        particlePooler.WarmUp();
        specialPooler.WarmUp();
    }

    public void PlayGame()
    {
        InitGame();
        gameCoroutine = StartCoroutine(GameRoutine());
    }

    public void ExitGame()
    {
        _score = 0;
        uiController.UpdateScoreText(0);
        AllObjectsPutPool();
        DestroyGrid();
        StopGameRoutine();
        GameState = GameState.NotStarted;
    }
    
    public void InitGame()
    {
        CreateGrid();
        CreateItems();
        SpecialItemsInBeginning();
    }

    public void SpecialItemsInBeginning()
    {
        for (int i = 0; i < _grid.ColCount; i++)
        {
            for (int j = 0; j < _grid.RowCount; j++)
            {
                Cell cell = _grid.GetCell(i, j);
                if (cell.Item.ItemType == ItemType.Special)
                {
                    specialItemsCount++;
                }
            }
        }

        specialItemsTotal = specialItemsCount;
        uiController.UpdateSpecialItemText(specialItemsCount);
    }

    public void UpdateSpecialUI()
    {
        uiController.UpdateSpecialItemText(specialItemsCount);
        if (specialItemsCount != 0) return;
        GameWin();
    }

    public void GameWin()
    {
        _gameState = GameState.Paused;
        specialItemsCount = 0;
        specialItemsTotal = 0;
        uiController.ShowWinUI();
        audioManager.StopMenuSound();
        PlayFireworksSound();
        WinParticlePlay();
    }

    private void CreateGrid()
    {
        // Instantiate grid prefab under this (Game) gameobject
        var gridGO = GameObject.Instantiate(gridPrefab, Vector3.zero, Quaternion.identity, transform);
        _grid = gridGO.GetComponent<Grid>();
        _grid.Init(DefaultColCount, DefaultRowCount);
    }

    public void DestroyGrid()
    {
        Destroy(_grid.gameObject);
    }

    public void CreateItems()
    {
        GridDecorator.DecorateGrid(_grid);
    }

    IEnumerator GameRoutine()
    {
        yield return null;

        while (true)
        {
            if (_gameState != GameState.Paused)
            {
                _gameState = fallCounter > 0 ? GameState.Fall : GameState.WaitingForInput;
            }
            
            if (fallCounter > 0)
            {
                while (fallCounter > 0)
                {
                    yield return null;
                }
                bool isDestroyThree = DestroyThreeItems();
                bool isSpecialItem = DestroySpecialItem();
                if (isDestroyThree || isSpecialItem)
                {
                    ExecuteAfterDestroy();
                }
            }
            yield return null;
        }
    }

    public void StopGameRoutine()
    {
        StopCoroutine(gameCoroutine);
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
    
    public void AllObjectsPutPool()
    {
        for (int i = 0; i < _grid.ColCount; i++)
        {
            for (int j = 0; j < _grid.RowCount; j++)
            {
                Cell cell = _grid.GetCell(i, j);
                itemPooler.Put(cell.Item.gameObject);
                cell.Item = null;
                cell.Col = 0;
                cell.Row = 0;
                cellPooler.Put(cell.gameObject);
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

    public void StartTrail(Vector3 position)
    {
        trailParticle.transform.position = _grid.MousePosToWorlPos(position);
        trailParticle.gameObject.SetActive(true);
        trailParticle.Play();
    }

    public void UpdateTrail(Vector3 position)
    {
        Vector3 trailPos = _grid.MousePosToWorlPos(position);
        trailParticle.transform.position = trailPos;
    }

    public void EndTrail()
    {
        trailParticle.Stop();
        trailParticle.gameObject.SetActive(false);
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
        
        if (!cell1.Item.CanSwipe())
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
            IncScore();
        }
        if (cutRightDownNeighborsOfCell.Count == 3)
        {
            audioManager.PlaySwipeSound();
            DestroyCells(cutRightDownNeighborsOfCell);
            isDestroy = true;
            IncScore();
        }

        if (isDestroy)
        {
            ExecuteAfterDestroy();
        }
        
        return true;
    }

    public bool DestroyThreeItems()
    {
        bool isDestroy = false;
        for (int i = 0; i < _grid.ColCount; i++)
        {
            for (int j = 0; j < _grid.RowCount; j++)
            {
                Cell cell = _grid.GetCell(i, j);
                if (cell.Item == null || !cell.Item.CanDestroy())
                {
                    continue;
                }
                List<Cell> threeNeighbors = _grid.FindCutNeighborsOfCell(cell, null);
                if (threeNeighbors.Count is 3 or 6 or 9 or 12)
                {
                    isDestroy = true;
                    DestroyCells(threeNeighbors);
                    IncScore();
                }
            }
        }
        
        return isDestroy;
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
    
    public void IncScore()
    {
        _score += 3;
        uiController.UpdateScoreText(_score);
    }

    public bool DestroySpecialItem()
    {
        bool isDestroy = false;
        for (int i = 0; i < _grid.ColCount; i++)
        {
            Cell cell = _grid.GetCell(i, 0);
            
            if (cell.Item == null || cell.Item.ItemType != ItemType.Special) continue;
            isDestroy = true;
            cell.DestroyItem();
            specialItemsCount--;
            UpdateSpecialUI();
        }
        return isDestroy;
    }

    public bool IsMouseOverGrid(Vector3 position)
    {
        var pos = _grid.MousePosToGridPos(position);
        if (pos.x < 0 || pos.x >= _grid.ColCount || pos.y < 0 || pos.y >= _grid.RowCount)
        {
            return false;
        }
        return true;
    }

    public void DestroyBooster(Vector3 position)
    {
        if (booster != 0)
        {
            var boosterPos = _grid.MousePosToGridPos(position);
            Cell cell = _grid.GetCell((int)boosterPos.x, (int)boosterPos.y);
            if (cell.Item.ItemType != ItemType.Special)
            {
                cell.DestroyItem();
                ExecuteAfterDestroy();
                booster--;
                uiController.boosterToggle.isOn = false;
                uiController.UpdateBoosterText(booster);
            }

            if (booster == 0)
            {
                uiController.boosterToggle.interactable = false;
            }
        }
        uiController.boosterToggle.isOn = false;
    }

    public void ChangeColorBooster(Vector3 position)
    {
        if (colorChange != 0)
        {
            var colorPos = _grid.MousePosToGridPos(position);
            Cell cell = _grid.GetCell((int) colorPos.x, (int) colorPos.y);
            cell.ChangeItemColor(selectedType);
            colorChange--;
            uiController.colorToggle.isOn = false;
            uiController.UpdateColorText(colorChange);
            DestroyThreeItems();
            ExecuteAfterDestroy();
            if (colorChange == 0)
            {
                uiController.colorToggle.interactable = false;
            }
        }
        uiController.colorToggle.isOn = false;
    }

    public void ColorPicker(ItemType type)
    {
        selectedType = type;
    }

    public bool IsDestroyBoosterOn()
    {
        return uiController.IsBoosterToggleOn();
    }

    public bool IsColorBoostOn()
    {
        return uiController.IsColorBoostToggleOn();
    }

    public void PlaySoundtrack()
    {
        audioManager.PlayMenuSound();
    }

    public void ClickSound()
    {
        audioManager.PlayClickSound();
    }

    public void ExplosionSound()
    {
        audioManager.PlayExplosionSound();
    }

    public void DropSound()
    {
        audioManager.PlayDropSound();
    }

    public void PlayFireworksSound()
    {
        audioManager.PlayFireworksSound();
    }

    public void StopFireworksSound()
    {
        audioManager.StopFireworksSound();
    }

    public void WinParticlePlay()
    {
        winParticle.gameObject.SetActive(true);
        winParticle.Play();
        StartCoroutine(WaitWinParticles());
    }

    public void WinParticleStop()
    {
        winParticle.Stop();
        winParticle.gameObject.SetActive(false);
    }

    IEnumerator WaitWinParticles()
    {
        yield return new WaitForSeconds(10f);
        WinParticleStop();
        StopFireworksSound();
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
