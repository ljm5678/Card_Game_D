using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardItem : MonoBehaviour  
{  
    /// <summary>  
    /// 卡牌扇形展开中心点  
    /// </summary>  
    public Vector3 root;  
    /// <summary>  
    /// 展开角度  
    /// </summary>  
    public float rot;  
    /// <summary>  
    /// 展开半径  
    /// </summary>  
    public float size;  
    /// <summary>  
    /// 动画速度  
    /// </summary>  
    public float animSpeed = 3;  
    /// <summary>  
    /// 高度值（决定卡牌层级）  
    /// </summary>  
    public float zPos=0;  
    SpriteRenderer spriteRenderer;

    //bool isdeleting = false;

  




    public  void RefreshData(Vector3 root,float rot, float size,float zPos)  
    {        
        this.root = root;  
        this.rot = rot;  
        this.size = size;  
        this.zPos = zPos;  
    }  





    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        
       
    }





    // Update is called once per frame  
    void Update()  
    {   
        spriteRenderer.sortingOrder = (int)zPos + 1;
    }  

    
    
    
    
    
    
    
    public IEnumerator Remove_Card(){
    
    //isdeleting = true;


    Vector3 destination = new Vector3(10, 0, 3);

    float closeEnough = 0.1f; // How close does the object need to be to consider it at the destination



    // Loop until the object is close enough to the destination
    while (Vector3.Distance(transform.position, destination) > closeEnough)
    {
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * animSpeed);
        yield return null;  // Pause here, return control to Unity, then continue next frame
    }
    
    
    Destroy(gameObject);  // Destroy the GameObject
    }




}