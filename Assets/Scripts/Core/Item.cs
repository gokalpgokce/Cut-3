using UnityEngine;

public class Item : MonoBehaviour
{ 
    public GameObject visual;
    private ItemType _itemType;
    
    public ItemType ItemType
    {
        get
        {
            return _itemType;
        }
        set
        {
            _itemType = value;
            Color color = ItemTypeToColor(_itemType);
            visual.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public Color ItemTypeToColor(ItemType type)
    {
        Color color;
        switch (type)
        {
            case ItemType.Magenta:
                color = Color.magenta; break;
            case ItemType.Blue:
                color = Color.blue; break;
            case ItemType.Yellow:
                color = Color.yellow; break;
            case ItemType.Red:
                color = Color.red; break;
            case ItemType.Green:
                color = Color.green; break;
            default:
                color = Color.black; break;
        }
        return color;
    }
}
