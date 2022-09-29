using System.Collections;
using UnityEditor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Col;
    public int Row;
    private const float FallSpeed = 5f;
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
            _item.visual.GetComponent<SpriteRenderer>().sortingOrder = Row;
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
        
        GameObject particle = Instantiate(destroyParticle,transform.position, Quaternion.identity);
        var particleSystem = particle.GetComponent<ParticleSystem>();
        particleSystem.Play();
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(Item.ItemTypeToColor(Item.ItemType));
        Item = null;
    }

    private IEnumerator ScalingDown(GameObject destroyItemGO, Vector3 endPos, float speed)
    {
        while (destroyItemGO.transform.localScale != endPos)
        {
            destroyItemGO.transform.localScale =
                Vector3.MoveTowards(destroyItemGO.transform.localScale, endPos, speed * Time.deltaTime);
            yield return null;
        }
        Destroy(destroyItemGO);
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
        float percent = 0f;
        float timeFactor = 1 / speed;
        while (fallItemGO.transform.position != endPos)
        {
            // fallItemGO.transform.position =
            //     Vector3.MoveTowards(fallItemGO.transform.position, endPos, speed * Time.deltaTime);
            
            
            // fallItemGO.transform.position = 
            //     Vector3.MoveTowards(fallItemGO.transform.position, endPos, Mathf.Sin(Time.deltaTime * speed));

            percent += Time.deltaTime * timeFactor;
            fallItemGO.transform.position = 
                Vector3.MoveTowards(fallItemGO.transform.position, endPos, Mathf.SmoothStep(0,1,percent));
            

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