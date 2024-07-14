using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    CardManager CM;
    // Start is called before the first frame update


    void Start()
    {

    }


    public void Initialize(int scene_id, ref List<CardItem> deck)
    {
        GameObject card_base = Resources.Load<GameObject>("Prefabs/CARD_BASE");
        if (card_base == null)
        {
            Debug.Log("¿¨Ãæ_null");
        }
        Vector3 root = new Vector3(0f, -0.2f, 5f);

        GameObject CM_obj = new GameObject();
        CardManager CM = CM_obj.AddComponent<CardManager>();
        CM.Initialize(ref root, ref card_base, deck);

    }

}
