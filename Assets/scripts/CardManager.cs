using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using public_func;


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
    public float size = 25f;  

    /// <summary>  
    /// 手牌列表  
    /// </summary>  
    public List<CardItem> Hand_list;
    /// <summary>  


    public List<CardItem> Deck_list;

    public Queue<CardItem> Deck_queue;

    public List<CardItem> Waste_list;

    /// <summary>  
    /// 最大手牌数量  
    /// </summary>  
    private int CardMaxCount= 10;  

    //main camera cache
    private Camera mainCamera;

    public int card_id = -1;

    //focused when the mouse is dragging
    private CardItem focused_item = null;

    //used for hover effect
    private int hovered_item_index = -1;
  
    private BoxCollider[] childColliders;

    public int draw_number = 5;

    


    public void Initialize(ref Vector3 rootpos, ref GameObject cardItem, List<CardItem> deck)
    {
        this.rootPos = rootpos;
        this.cardItem = cardItem;
        this.Deck_list = deck;
        Hand_list = new List<CardItem>();
        Deck_queue = new Queue<CardItem>(Deck_list);
        Waste_list = new List<CardItem>();
    }

    void Start()  
    {         
        mainCamera = Camera.main;
       
    }    
    
    
    




    // Update is called once per frame  

    void Update()  
    {   

        RefereshCard();       
        TaskItemDetection();  
        

    }   





    /// <summary>  
    /// 添加卡牌  
    /// </summary>  

    public void Draw()  
    {        
        if (Hand_list == null)  
        {            
            Hand_list = new List<CardItem>();  
        }  
        if (Hand_list.Count >= CardMaxCount)  
        {            
            return;  
        }        
        
        if (Deck_queue.Count == 0 && Waste_list.Count == 0){
            return;
        }

        if(Deck_queue.Count == 0){
            Shuffle<CardItem>(ref Waste_list, ref Deck_queue);
            Debug.Log("Shuffled");
        }
        GameObject item = Instantiate(cardItem, this.transform);
        item.transform.localPosition = new Vector3(-10, 25, 10);

        Hand_list.Add(CardFactory.CreateCard(ref item, Deck_queue.Dequeue().id));
        //CardItem newCard = CardFactory.CreateCard(ref item, Deck_queue.Dequeue().id);
        //CardItem newCard = item.AddComponent<Card_1>();
        //newCard.Initialize(Deck_queue.Dequeue().id);
        //Hand_list.Add(newCard);

    }





    /// <summary>  
    /// 手牌状态刷新  
    /// </summary>  

    public void RefereshCard()  
    {        
        if (Hand_list == null || Hand_list.Count == 0)  
        {            
           return;  
        }
        

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Create a ray from the mouse position
        
        
        childColliders = GetComponentsInChildren<BoxCollider>();    //read all Collider components from the children list

        if(hovered_item_index != -1){
            if(!check_if_hovered (ref ray, ref childColliders, hovered_item_index) || focused_item != null){
                hovered_item_index = -1;
            }
        }


        float start_x = rootPos.x - 0.6f * (float) Mathf.Pow((float)(Hand_list.Count - 1), 0.8f);
        

        for (int i = 0; i < Hand_list.Count; i++)  
        {   
            
            
            
            
            
            bool ishovering = check_if_hovered (ref ray, ref childColliders, i);
            


            
            float targetx;

            //if there is only one item
            if (start_x != rootPos.x){

                //default x position
                    targetx = (start_x + i * ((rootPos.x - start_x) * 2) / (Hand_list.Count - 1));
                
                //adding effect if a card is being hovered
                if(hovered_item_index != -1 && hovered_item_index != i){
                    
                    targetx = targetx + 1f / (i - hovered_item_index) * 0.3f;

                }
                


            }
            else{
                targetx = rootPos.x;
            }
           
           
            //default y position

            float targety = (float) rootPos.y - Mathf.Pow((float)(Mathf.Abs(Hand_list[i].transform.position.x - rootPos.x) * 0.2), 2f) - 0.2f;


            //default rotation
            Quaternion rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, (rootPos.x - Hand_list[i].transform.position.x) * 5f));
            
            //is the card being hovered || it's being held
            if ((ishovering || focused_item == Hand_list[i])){
               
                childColliders[i].size = new Vector3 (6.3f, 9f, childColliders[i].size.z);

                rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, 0));
                
                targety = Camera.main.transform.position.y - 1.8f;
                
                Hand_list[i].transform.localScale = Vector3.Lerp(Hand_list[i].transform.localScale,  new Vector3(0.25f, 0.25f, 1f), Time.deltaTime * 50f);
                
                Hand_list[i].RefreshData(20);
                
                if (Input.GetMouseButtonDown(0) && focused_item == null && Hand_list[i].Type != CardItem.CardType.Not_Playable){
                    focused_item = Hand_list[i];
                }

                if(ishovering && focused_item == null){
                    hovered_item_index = i;
                }
            }
            else{           //otherwise it treat the card normally 
                Hand_list[i].transform.localScale = Vector3.Lerp(Hand_list[i].transform.localScale,  new Vector3(0.2f, 0.2f, 0.2f), Time.deltaTime * 50f);

                childColliders[i].size = new Vector3 (5.75f, 8.5f, childColliders[i].size.z);

                Hand_list[i].RefreshData(i);
            }

            


            if(focused_item != Hand_list[i]){

                //it goes to where it should go
                Hand_list[i].transform.position = Vector3.Lerp(Hand_list[i].transform.position, new Vector3(targetx, targety, rootPos.z), Time.deltaTime * 30);
            }
            else{
                
                if (Hand_list[i].Type == CardItem.CardType.No_Target){
                    Hand_list[i].transform.position = Vector3.Lerp(Hand_list[i].transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Hand_list[i].transform.position.z)), Time.deltaTime * 30);

                }
                else if(Hand_list[i].Type == CardItem.CardType.Target){
                    Hand_list[i].transform.position = Vector3.Lerp(Hand_list[i].transform.position, new Vector3(rootPos.x, targety, rootPos.z), Time.deltaTime * 30);

                }
            
            }



            //changing the angle
            Hand_list[i].transform.rotation = Quaternion.RotateTowards(Hand_list[i].transform.rotation, rotationQuaternion, Time.deltaTime * 1000);  
                

        }  
    }    






    /// <summary>  
    /// 销毁卡牌  
    /// </summary>  

    public  void RemoveCard()  
    {        
        if (Hand_list==null || Hand_list.Count == 0)  
        {
            return;  
        }  
        CardItem item = Hand_list[Hand_list.Count - 1];
        Hand_list.Remove(item); 
        StartCoroutine(item.Remove_Card());
    }    






	/// <summary>  
    /// 销毁卡牌  
    /// </summary>  
    /// <param name="item"></param>  

    public  void RemoveCard(CardItem item)  
    {        
        if (Hand_list==null || Hand_list.Count == 0)  
        {
	        return;  
        }   
        Hand_list.Remove(item);
        Waste_list.Add(item);
        StartCoroutine(item.Remove_Card());
    }  








  
          






    /// <summary>  
    /// 玩家操作检测  
    /// </summary>  

    public void TaskItemDetection()  
    {  
        
        
        hotkeys();
        
        if (Input.GetMouseButtonDown(0) && focused_item != null){

            if(Input.mousePosition.y >= Screen.height / 2.5){
                play(focused_item);
                focused_item = null;
            }
            
        }

        if(Input.GetMouseButtonDown(1)){
            focused_item = null;
        }




        //debug
       if (Input.GetKeyDown(KeyCode.A))  
        {         
           Draw();  
        }    
        if (Input.GetKeyDown(KeyCode.D) && Hand_list != null && Hand_list.Count != 0)  
        {         
           RemoveCard(Hand_list[Hand_list.Count - 1]);
        }


        
     }  


     public void play(CardItem played_card){
        RemoveCard(played_card);
     }






    private bool check_if_hovered (ref Ray ray, ref BoxCollider[] childColliders, int i){
        bool ishovering = false;
        RaycastHit hit;
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width && Input.mousePosition.y >= 0 && Input.mousePosition.y <= Screen.height)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == childColliders[i]) // Check if the ray hits this object's collider
                {
                    ishovering = true;
                }
            }
        }
        return ishovering;
    }   

    public void hotkeys(){
        
        int i = 20;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            i = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            i = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            i = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            i = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            i = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            i = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            i = 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            i = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            i = 9;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            i = 10;
        }

        if (Hand_list.Count >= i){
            if (focused_item != Hand_list[i - 1]){
                focused_item = Hand_list[i - 1];
            }
            else{
                focused_item = null;
            }
        }

    }

    public void Shuffle<T>(ref List<T> list, ref Queue<T> queue)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            // Random index from 0 to i
            int j = rng.Next(i + 1);

            // Swap elements
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        // Enqueue all elements from the shuffled list
        foreach (T item in list)
        {
            queue.Enqueue(item);
        }
        list.Clear();
    }

}