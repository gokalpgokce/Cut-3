using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject _mainUIGO;
    
    public void ShowMainUI() 
    {
        _mainUIGO.SetActive(true);
    }
}