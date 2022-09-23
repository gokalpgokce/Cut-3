using System.Collections;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Col;
    public int Row;
    private const float FallSpeed = 5f;
    public GameObject visual;
    [SerializeField] private Item _item;

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

    public void FallItem(Cell fallCell, Cell emptyCell)
    {
        fallCell.Item.gameObject.transform.SetParent(emptyCell.transform,true);
        emptyCell.Item = fallCell.Item;
        fallCell.Item = null;
        StartCoroutine(FallRoutine(emptyCell.Item.gameObject, emptyCell.transform.position,FallSpeed));
    }

    private IEnumerator FallRoutine(GameObject fallItemGO, Vector3 endPos, float speed)
    {
        while (fallItemGO.transform.position != endPos)
        {
            fallItemGO.transform.position =
                Vector3.MoveTowards(fallItemGO.transform.position, endPos, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void SpawnItem()
    {
        StartCoroutine(FallRoutine(Item.gameObject, transform.position, FallSpeed));
    }

    public override string ToString()
    {
        return string.Format("col:{0},row:{1} ", Col, Row);
    }
}