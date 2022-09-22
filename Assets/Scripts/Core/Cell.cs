using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Col;
    public int Row;
    public GameObject visual;
    private Item _item;

    public void Init(int col, int row)
    {
        Col = col;
        Row = row;
    }

    public Item Item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
        }
    }

    public override string ToString()
    {
        return string.Format("col:{0},row:{1} ", Col, Row);
    }
}