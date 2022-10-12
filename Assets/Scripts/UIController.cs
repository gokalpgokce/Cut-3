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
    public GameObject optionsUIGO;
    public Toggle soundToggle;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI specialItemsText;
    
    public void UpdateScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateSpecialItemText(int count)
    {
        specialItemsText.text = "Special Items: " + count;
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

    public void ShowOptionsUI()
    {
        optionsUIGO.SetActive(true);
    }

    public void HideOptionsUI()
    {
        optionsUIGO.SetActive(false);
    }
    
    public void OnPlayClicked()
    {
        HideMainUI();
        ShowGameUI();
        Game.Instance.PlayGame();
        Game.Instance.ClickSound();
    }

    public void OnPauseClicked()
    {
        HideGameUI();
        ShowPausedUI();
        Game.Instance.ClickSound();
    }

    public void OnBackClicked()
    {
        ShowMainUI();
        HideOptionsUI();
        Game.Instance.ClickSound();
    }

    public void OnOptionsClicked()
    {
        ShowOptionsUI();
        HideMainUI();
        Game.Instance.ClickSound();
    }

    public void OnYesClicked()
    {
        HidePausedUI();
        HideGameUI();
        ShowMainUI();
        _main.FinishGame();
        Game.Instance.ClickSound();
    }
    public void OnNoClicked()
    {
        HidePausedUI();
        ShowGameUI();
        _main.ResumeGame();
        Game.Instance.ClickSound();
    }

    public bool isToggleOn()
    {
        return soundToggle.isOn;
    }

    public void ToggleClicked()
    {
        if (isToggleOn())
        {
            AudioListener.volume = 1;
            Game.Instance.ClickSound();
        }
        else
        {
            AudioListener.volume = 0;
        }
    }
}