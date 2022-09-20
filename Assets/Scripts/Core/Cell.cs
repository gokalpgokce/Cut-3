using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Row;
    public int Col;
    public GameObject visual;
    private CellType _cellType;
    
    public void Init(int col, int row)
    {
        Row = row;
        Col = col;
    }

    public CellType CellType
    {
        get
        {
            return _cellType;
        }
        set
        {
            _cellType = value;
            Color color = CellTypeToColor(_cellType);
            visual.GetComponent<SpriteRenderer>().color = color;
        }
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