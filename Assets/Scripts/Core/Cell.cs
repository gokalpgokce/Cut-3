using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Row;
    public int Col;
    public GameObject visual;
    public Color cellType;
    
    public void Init(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public void ChangeCellColor(Color colorType)
    {
        // change cell color
        visual.GetComponent<SpriteRenderer>().color = colorType;
        // cell type array
        
    }
}