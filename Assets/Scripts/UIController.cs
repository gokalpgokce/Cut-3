using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    //private GameState _gameState;
    [Header("Scene References")]
    public GameObject mainUIGO;
    public GameObject gameUIGO;
    
    public void ShowMainUI(bool isMenu)
    {
        mainUIGO.SetActive(true);
        if (isMenu)
        {
            mainUIGO.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            mainUIGO.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void HideMainUI()
    {
        mainUIGO.SetActive(false);
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

    public void OnMenuClicked()
    {
        HideGameUI();
        ShowMainUI(true);
    }

    public void OnResumeClicked()
    {
        HideMainUI();
        ShowGameUI();
    }
}