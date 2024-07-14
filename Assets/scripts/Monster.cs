using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float health;
    public float attack;
    public int position;
    public int id;



    public void Initialize(int position, int id){
        this.position = position;
        this.id = id;

        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Monster/" + id.ToString());
        if (position == 0)
        {
            // left
            transform.position = new Vector3(-2.5f, 0.5f, 0);
        }
        else if (position == 1)
        { // right
            transform.position = new Vector3(2.5f, 0.5f, 0);
        }
        else if (position == 2)
        { // top
            transform.position = new Vector3(0, 3.5f, 0);
        }
        else if (position == 3)
        { // bottom
            transform.position = new Vector3(0, -2.5f, 0);
        }

    }



    void Start(){
        

    }
    
    void Update(){


    }



    private void renew(){
        
    }

    
}
