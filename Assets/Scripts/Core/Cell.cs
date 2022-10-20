using System.Collections;
using UnityEditor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Col;
    public int Row;
    private const float FallDuration = 1f;
    private const float ScalingSpeed = 3f;
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
        if (Item.ItemType == ItemType.Special)
        {
            SpecialParticlesPlay();
            return;
        }
        ParticlesPlay();
    }

    private void ParticlesPlay()
    {
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
    
    private void SpecialParticlesPlay()
    {
        GameObject particle = Game.Instance.specialPooler.Get();
        particle.transform.position = transform.position;
        particle.transform.rotation = Quaternion.identity;
        StartCoroutine(WaitSpecialParticle(particle));
        var particleSystem = particle.GetComponent<ParticleSystem>();
        Game.Instance.ExplosionSound();
        particleSystem.Play();
        Item = null;
    }

    private IEnumerator WaitParticle(GameObject particle)
    {
        yield return new WaitForSeconds(0.2f);
        Game.Instance.particlePooler.Put(particle);
    }
    private IEnumerator WaitSpecialParticle(GameObject particle)
    {
        yield return new WaitForSeconds(2f);
        Game.Instance.specialPooler.Put(particle);
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
        Fall(emptyCell.Item.gameObject, emptyCell.transform.position.y,FallDuration);
    }
    
    private void Fall(GameObject fallGO, float to, float time)
    {
        Game.Instance.fallCounter++;
        Invoke("DropSoundEffect",0.3f);
        LeanTween.moveY(fallGO, to, time).setEaseOutBounce().setOnComplete(FallCounterDescent);
    }

    public void DropSoundEffect()
    {
        Game.Instance.DropSound();
    }

    private void FallCounterDescent()
    {
        Game.Instance.fallCounter--;
    }

    public void SpawnItem()
    {
        GameObject itemGO = Item.gameObject;
        itemGO.transform.localScale = Vector3.zero;
        StartCoroutine(ScalingUp(itemGO, Vector3.one, ScalingSpeed));
        Fall(Item.gameObject, transform.position.y,FallDuration);
    }

    public override string ToString()
    {
        return string.Format("col:{0},row:{1} ", Col, Row);
    }
}