using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public List<CardItem> Deck_list;
    public GameObject cardItem;
    public int scene_id;


    // Start is called before the first frame update
    void Start()
    {   

        Deck_list = new List<CardItem>();

        scene_id = 0;





        for (int i = 0; i < 5; i++)
        {
            GameObject item = Instantiate(cardItem, this.transform);
            item.transform.localPosition = new Vector3(-10, 25, 10);


            //CardItem newCard = item.AddComponent<Card_1>();
            //CardItem newCard = CardFactory.CreateCard(ref item, i);
            Deck_list.Add(CardFactory.CreateCard(ref item, i));
        }

        GameObject BM_obj = new GameObject();
        BattleManager BM = BM_obj.AddComponent<BattleManager>();
        BM.Initialize(scene_id, ref Deck_list);
    }

    
}
