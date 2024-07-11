using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardManager : MonoBehaviour  
{  
    /// <summary>  
    /// 卡牌起始位置  
    /// </summary>  
    public Vector3 rootPos=new Vector3(0,0,0);  
    /// <summary>  
    /// 卡牌对象  
    /// </summary>  
    public GameObject cardItem;  
    /// <summary>  
    /// 扇形半径  
    /// </summary>  
    public float size =30f;  
    /// <summary>  
    /// 卡牌出现最大位置  
    /// </summary>  
    private float minPos = 1.365f;  
    /// <summary>  
    /// 卡牌出现最小位置  
    /// </summary>  
    private float maxPos = 1.73f;  
    /// <summary>  
    /// 手牌列表  
    /// </summary>  
    public List<CardItem> cardList;  
    /// <summary>  
    /// 手牌位置  
    /// </summary>  
    private List<float> rotPos;  
    /// <summary>  
    /// 最大手牌数量  
    /// </summary>  
    private int CardMaxCount= 10;  

    private Camera mainCamera;

    private CardItem focused_item = null;
  






    void Start()  
    {        
        InitCard();  
        mainCamera = Camera.main;
    }    
    
    
    
    
    /// <summary>  
    /// 数据初始化  
    /// </summary>  

    public  void InitCard()  
    {  
          rotPos=InitRotPos(CardMaxCount);  
    }    
  
  
  
  
    /// <summary>  
    /// 初始化位置  
    /// </summary>  
    /// <param name="count"></param>    
    /// <param name="interval"></param>    
    /// <returns></returns>    

    public List<float> InitRotPos(int count)  
    {        
        List<float> rotPos=new List<float>();  
        float interval = (maxPos - minPos)/count;  
        for (int i = 0; i < count; i++)  
        {            
            float nowPos = maxPos - interval * i;  
            rotPos.Add(nowPos);  
        }        
        return rotPos;  
    }  





    // Update is called once per frame  

    void Update()  
    {        
        TaskItemDetection();  
        RefereshCard();  

    }   





    /// <summary>  
    /// 添加卡牌  
    /// </summary>  

    public void AddCard()  
    {        
        if (cardList == null)  
        {            
            cardList = new List<CardItem>();  
        }  
        if (cardList.Count >= CardMaxCount)  
        {            
            Debug.Log("手牌数量上限");  
            return;  
        }        
        GameObject item = Instantiate(cardItem, this.transform);  
        item.transform.localPosition = new Vector3(-10, 25, 10);
        CardItem text = item.GetComponent<CardItem>();  
        text.RefreshData(rootPos, 0, 0, 0 );  

        // 加载图片资源


        cardList.Add(text);  
    }  





    /// <summary>  
    /// 手牌状态刷新  
    /// </summary>  

    public void RefereshCard()  
    {        
        if (cardList==null)  
        {            
           return;  
        }
        

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Create a ray from the mouse position
        RaycastHit hit;
        
        BoxCollider[] childColliders = GetComponentsInChildren<BoxCollider  >();    //read all Collider components from the children list


        float start_x = rootPos.x - 0.6f * (float) Mathf.Pow((float)(cardList.Count - 1), 0.8f);
        

        for (int i = 0; i < cardList.Count; i++)  
        {   
            bool ishovering = false;
            

            if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width && Input.mousePosition  .y >= 0 && Input.mousePosition.y <= Screen.height)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider == childColliders[i]) // Check if the ray hits this object's collider
                    {
                        ishovering = true;
                    }
                }
            }
            
            
            
            float targetx;

            //if there is only one item
            if (start_x != rootPos.x){
                targetx = (start_x + i * ((rootPos.x - start_x) * 2) / (cardList.Count - 1));
            }
            else{
                targetx = rootPos.x;
            }
           
           
            //default y position

            float targety = (float) rootPos.y - Mathf.Pow((float)(Mathf.Abs(cardList[i].transform.position.x - rootPos.x) * 0.2), 2f) - 0.2f;


            //default rotation
            Quaternion rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, (rootPos.x - cardList[i].transform.position.x) * 6f));
            
            //is the card being hovered || it's being held
            if (ishovering || focused_item == cardList[i]){
               
                childColliders[i].size = new Vector3 (6.5f, childColliders[i].size.y, childColliders[i].size.z);

                rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, 0));
                
                targety = Camera.main.transform.position.y - 1.9f;
                
                cardList[i].transform.localScale = Vector3.Lerp(cardList[i].transform.localScale,  new Vector3(0.25f, 0.25f, 1f), Time.deltaTime * 50f);
                
                cardList[i].RefreshData(rootPos,rotPos[i],size,20);
                
                if (Input.GetMouseButton(0) && focused_item == null){
                    focused_item = cardList[i];
                }
            }
            else{           //otherwise it treat the card normally 
                cardList[i].transform.localScale = Vector3.Lerp(cardList[i].transform.localScale,  new Vector3(0.2f, 0.2f, 0.2f), Time.deltaTime * 50f);

                cardList[i].RefreshData(rootPos,rotPos[i],size,i);
            }

            


            if(focused_item != cardList[i]){

                //it goes to where it should go
                cardList[i].transform.position = Vector3.Lerp(cardList[i].transform.position, new Vector3(targetx, targety, rootPos.z), Time.deltaTime * 30);
            }
            else{

                //it goes for the mouse
                cardList[i].transform.position = Vector3.Lerp(cardList[i].transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cardList[i].transform.position.z)), Time.deltaTime * 30);
            }



            //changing the angle
            cardList[i].transform.rotation = Quaternion.RotateTowards(cardList[i].transform.rotation, rotationQuaternion, Time.deltaTime * 200);  
                

        }  
    }    






    /// <summary>  
    /// 销毁卡牌  
    /// </summary>  

    public  void RemoveCard()  
    {        
        if (cardList==null || cardList.Count == 0)  
        {
            return;  
        }  
        CardItem item = cardList[cardList.Count - 1];
        cardList.Remove(item); 
        StartCoroutine(item.Remove_Card());
    }    






	/// <summary>  
    /// 销毁卡牌  
    /// </summary>  
    /// <param name="item"></param>  

    public  void RemoveCard(CardItem item)  
    {        
        if (cardList==null || cardList.Count == 0)  
        {
	        return;  
        }   
        cardList.Remove(item);
        StartCoroutine(item.Remove_Card());
    }  








  
          






    /// <summary>  
    /// 玩家操作检测  
    /// </summary>  

    public void TaskItemDetection()  
    {  
        
        
        
        
        if (!Input.GetMouseButton(0)){
            if(focused_item != null){

                if(Input.mousePosition.y >= Screen.height / 2.5){
                    play(focused_item);
                }
                

                
            }

            focused_item = null;
        }







        //debug
       if (Input.GetKeyDown(KeyCode.A))  
        {         
           AddCard();  
        }    
        if (Input.GetKeyDown(KeyCode.D) && cardList != null && cardList.Count != 0)  
        {         
           RemoveCard(cardList[cardList.Count - 1]);
        }           
     }  


     public void play(CardItem played_card){
        RemoveCard(played_card);
     }
}
