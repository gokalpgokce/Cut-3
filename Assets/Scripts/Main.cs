using UnityEngine;

public class Main : MonoBehaviour
{
    public UIController _uiController;
    public Camera cam;
    
    // First Entry Point - Game Should Start From Here
    void Start()
    {
        Application.targetFrameRate = 60;
        StartGame();
    }
    
    private void StartGame()
    {
        CalculateOrthographicSize();
        ShowMainUI();
        HideGameUI();
    }
    
    private void ShowMainUI()
    {
        _uiController.ShowMainUI(false);
    }
    private void HideGameUI()
    {
        _uiController.HideGameUI();
    }
    
    private void CalculateOrthographicSize()
    {
        float size = (Screen.height / 2.0f * 5.0f) / (Screen.width / 2.0f);
        size = Mathf.Max(size, Game.DefaultRowCount / 2.0f + 1.0f);
        cam.orthographicSize = size;
    }
}
