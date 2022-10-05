using System.Collections;
using UnityEditor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Col;
    public int Row;
    private const float FallDuration = 2f;
    private const float ScalingSpeed = 3f;
    public GameObject destroyParticle;
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
            if (_item != null)
            {
                _item.visual.GetComponent<SpriteRenderer>().sortingOrder = Row;
            }
        }
    }

    public bool IsSameType(Cell otherCell)
    {
        return otherCell.Item != null && Item != null && otherCell.Item.ItemType == Item.ItemType;
    }

    public void DestroyItem()
    {
        // TODO: fix here. kare gelmesi durumunda cutLeft ve cutRight iki if e de giriyor. CheckValidCut()
        if (Item == null)
        {
            return;
        }
        
        Item.visual.GetComponent<SpriteRenderer>().sortingOrder = -1;
        StartCoroutine(ScalingDown(Item.gameObject, Vector3.zero, ScalingSpeed));

        GameObject particle = Game.Instance.particlePooler.Get();
        particle.transform.position = transform.position;
        particle.transform.rotation = Quaternion.identity;
        StartCoroutine(WaitParticle(particle));
        var particleSystem = particle.GetComponent<ParticleSystem>();
        particleSystem.Play();
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(Item.ItemTypeToColor(Item.ItemType));
        Item = null;
    }

    private IEnumerator WaitParticle(GameObject particle)
    {
        yield return new WaitForSeconds(0.2f);
        Game.Instance.particlePooler.Put(particle);
    }

    private IEnumerator ScalingDown(GameObject destroyItemGO, Vector3 endPos, float speed)
    {
        while (destroyItemGO.transform.localScale != endPos)
        {
            destroyItemGO.transform.localScale =
                Vector3.MoveTowards(destroyItemGO.transform.localScale, endPos, speed * Time.deltaTime);
            yield return null;
        }
        Game.Instance.itemPooler.Put(destroyItemGO);
    }
    
    private IEnumerator ScalingUp(GameObject spawnItemGO, Vector3 endPos, float speed)
    {
        while (endPos != spawnItemGO.transform.localScale)
        {
            spawnItemGO.transform.localScale =
                Vector3.MoveTowards(spawnItemGO.transform.localScale, endPos, speed * Time.deltaTime*2);
            yield return null;
        }
    }

    public void FallItem(Cell fallCell, Cell emptyCell)
    {
        fallCell.Item.gameObject.transform.SetParent(emptyCell.transform,true);
        emptyCell.Item = fallCell.Item;
        fallCell.Item = null;
        StartCoroutine(FallRoutine(emptyCell.Item.gameObject, emptyCell.transform.position,FallDuration));
    }

    private IEnumerator FallRoutine(GameObject fallItemGO, Vector3 endPos, float duration)
    {
        float percent = 0f;
        float timeFactor = 1 / duration;
        while (fallItemGO.transform.position != endPos)
        {
            percent += Time.deltaTime * timeFactor;
            fallItemGO.transform.position = 
                Vector3.MoveTowards(fallItemGO.transform.position, endPos, Mathf.SmoothStep(0,1,percent));
            
            yield return null;
        }
    }

    public void SpawnItem()
    {
        GameObject itemGO = Item.gameObject;
        itemGO.transform.localScale = Vector3.zero;
        StartCoroutine(ScalingUp(itemGO, Vector3.one, ScalingSpeed));
        StartCoroutine(FallRoutine(Item.gameObject, transform.position, FallDuration));
    }

    public override string ToString()
    {
        return string.Format("col:{0},row:{1} ", Col, Row);
    }
}