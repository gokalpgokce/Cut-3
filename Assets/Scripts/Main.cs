using UnityEngine;

public class Main : MonoBehaviour
{
    public UIController _uiController;
    public Game _game;
    
    // First Entry Point - Game Should Start From Here
    void Start()
    {
        StartGame();
    }
    
    private void StartGame()
    {
        ShowMainUI();
    }
    
    private void ShowMainUI()
    {
        _uiController.ShowMainUI();
    }
}
