using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Row;
    public int Col;
    public GameObject visual;
    private CellType _cellType;
    
    public void Init(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public void ChangeCellType(CellType cellType)
    {
        // change cell color
        _cellType = cellType;
        Color color = CellTypeToColor(_cellType);
        visual.GetComponent<SpriteRenderer>().color = color;
    }

    public Color CellTypeToColor(CellType type)
    {
        Color color;
        switch (type)
        {
            case CellType.Magenta:
                color = Color.magenta; break;
            case CellType.Blue:
                color = Color.blue; break;
            case CellType.Yellow:
                color = Color.yellow; break;
            case CellType.Red:
                color = Color.red; break;
            case CellType.Green:
                color = Color.green; break;
            default:
                color = Color.black; break;
        }
        return color;
    }
}