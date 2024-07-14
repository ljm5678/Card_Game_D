using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardItem : MonoBehaviour
{
    public float animSpeed = 30;
    public float zPos = 0;
    public SpriteRenderer spriteRenderer;
    public int id;

    public enum CardType
    {
        Target,
        No_Target,
        Not_Playable
    }

    public CardType Type;

    
    
    
    
    public virtual void RefreshData(float zPos)
    {
        this.zPos = zPos;
        spriteRenderer.sortingOrder = (int)zPos + 1;
    }

    public void Initialize(int id)
    {
        this.id = id;

    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    
    
    public virtual void Play()
    {
        // Default play implementation (can be overridden)
    }

    public virtual void Load()
    {
        
    }
    
    
    
    public IEnumerator Remove_Card()
    {
        Vector3 destination = new Vector3(7, 0, 5);
        float closeEnough = 0.1f;

        while (Vector3.Distance(transform.position, destination) > closeEnough)
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * animSpeed);
            yield return null;
        }

        Destroy(gameObject);
    }
}
