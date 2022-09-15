using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Row;
    public int Col;
    
    public void Init(int row, int col)
    {
        Row = row;
        Col = col;
    }
}