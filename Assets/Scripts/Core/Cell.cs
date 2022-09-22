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

    public bool IsSameType(Cell otherCell)
    {
        return otherCell.Item != null && Item != null && otherCell.Item.ItemType == Item.ItemType;
    }

    public void DestroyItem()
    {
        Destroy(Item.gameObject);
        Item = null;
    }

    public override string ToString()
    {
        return string.Format("col:{0},row:{1} ", Col, Row);
    }
}