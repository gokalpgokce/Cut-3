using UnityEngine;

public class Main : MonoBehaviour
{
    public UIController _uiController;
    public Camera cam;
    
    // First Entry Point - Game Should Start From Here
    void Start()
    {
        Application.targetFrameRate = 60;
        _uiController.background.gameObject.SetActive(true);
        _uiController.CalculateBackGround();
        StartGame();
    }
    
    private void StartGame()
    {
        CalculateOrthographicSize();
        ShowMainUI();
        HideGameUI();
        Game.Instance.WarmUpPools();
    }

    public void PauseGame()
    {
        Game.Instance.GameState = GameState.Paused;
    }

    public void ResumeGame()
    {
        Game.Instance.GameState = GameState.WaitingForInput;
    }

    public void FinishGame()
    {
        Game.Instance.ExitGame();
    }
    
    private void ShowMainUI()
    {
        _uiController.ShowMainUI();
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
