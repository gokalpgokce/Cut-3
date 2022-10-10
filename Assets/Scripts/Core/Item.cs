using UnityEngine;
using UnityEngine.U2D;

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
            Sprite sprite = ItemTypeToSprite(_itemType);
            visual.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
    
    public Sprite ItemTypeToSprite(ItemType type)
    {
        Sprite sprite;
        switch (type)
        {
            case ItemType.Blue:
                sprite = ResourceManager.Instance.itemSprites[0]; break;
            case ItemType.Darkblue:
                sprite = ResourceManager.Instance.itemSprites[1]; break;
            case ItemType.Green:
                sprite = ResourceManager.Instance.itemSprites[2]; break;
            case ItemType.Orange:
                sprite = ResourceManager.Instance.itemSprites[3]; break;
            case ItemType.Purple:
                sprite = ResourceManager.Instance.itemSprites[4]; break;
            case ItemType.Red:
                sprite = ResourceManager.Instance.itemSprites[5]; break;
            case ItemType.Yellow:
                sprite = ResourceManager.Instance.itemSprites[6]; break;
            case ItemType.Special:
                sprite = ResourceManager.Instance.itemSprites[7]; break;
            default:
                sprite = ResourceManager.Instance.itemSprites[0]; break;
        }
        return sprite;
    }

    public Color ItemTypeToColor(ItemType type)
    {
        Color color;
        switch (type)
        {
            case ItemType.Blue:
                color = new Color(0, 215, 252); break;
            case ItemType.Darkblue:
                color = new Color(0, 63, 221); break;
            case ItemType.Green:
                color = new Color(0, 235, 0); break;
            case ItemType.Orange:
                color = new Color(255, 109, 0); break;
            case ItemType.Purple:
                color = new Color(238, 0, 255); break;
            case ItemType.Red:
                color = new Color(255, 0, 0); break;
            case ItemType.Yellow:
                color = new Color(255, 218, 0); break;
            default:
                color = new Color(0, 0, 0); break;
        }
        return color;
    }
}
