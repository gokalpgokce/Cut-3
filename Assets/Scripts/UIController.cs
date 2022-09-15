using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject mainUIGO;
    
    public void ShowMainUI()
    {
        mainUIGO.SetActive(true);
    }
    
    public void HideMainUI()
    {
        mainUIGO.SetActive(false);
    }
    
    public void OnPlayClicked()
    {
        HideMainUI();
        
        Game.Instance.PlayGame();
    }
}