using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Main _main;
    [Header("Scene References")]
    public GameObject mainUIGO;
    public GameObject gameUIGO;
    public GameObject pausedUIGO;
    public TextMeshProUGUI scoreText;
    
    public void UpdateScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void ShowMainUI()
    {
        mainUIGO.SetActive(true);
    }
    
    public void HideMainUI()
    {
        mainUIGO.SetActive(false);
    }

    public void ShowPausedUI()
    {
        pausedUIGO.SetActive(true);
        _main.PauseGame();
    }
    
    public void HidePausedUI()
    {
        pausedUIGO.SetActive(false);
    }
    
    public void ShowGameUI()
    {
        gameUIGO.SetActive(true);
    }
    
    public void HideGameUI()
    {
        gameUIGO.SetActive(false);
    }
    
    public void OnPlayClicked()
    {
        HideMainUI();
        ShowGameUI();
        Game.Instance.PlayGame();
    }

    public void OnPauseClicked()
    {
        HideGameUI();
        ShowPausedUI();
    }

    public void OnYesClicked()
    {
        HidePausedUI();
        HideGameUI();
        ShowMainUI();
        _main.FinishGame();
    }
    public void OnNoClicked()
    {
        HidePausedUI();
        ShowGameUI();
        _main.ResumeGame();
    }
}