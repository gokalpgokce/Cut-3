using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject mainUIGO;
    
    public TMP_InputField rowInputField;
    public TMP_InputField colInputField;
    
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
        
        int rowCount = 9;
        int colCount = 9;
        try
        {
            rowCount = int.Parse(rowInputField.text);
            colCount = int.Parse(colInputField.text);
        }
        catch (System.Exception)
        {
            Debug.Log("Invalid row col count.");
        }
        
        if (rowCount < 3 || colCount < 3 || rowCount > 9 || colCount > 9)
        {
            Debug.Log("Row Col count should be between 3-9");
        }
        
        rowCount = Mathf.Clamp(rowCount, 3, 9);
        colCount = Mathf.Clamp(colCount, 3, 9);
        
        Game.Instance.PlayGame();
    }
}