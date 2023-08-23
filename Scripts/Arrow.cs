using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public GameObject spawnOrigin;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
        gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
    void OnEnable()
    {
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D obj)
    {
        if(obj.gameObject.tag.Equals("Enemy") /*&& spawnOrigin.name.Equals("Player")*/)
        {
            Destroy(obj.gameObject);
            Destroy(gameObject);
        }
        /*else if(obj.gameObject.tag.Equals("Enemy"))
        {
            Physics2D.IgnoreCollision(obj.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        }*/
  /*      else if(obj.gameObject.tag.Equals("Player"))
        {
            /*obj.gameObject.SendMessage("DecreaseHealth");
            Destroy(gameObject);
            Physics2D.IgnoreCollision(obj.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            
        } */
        //this.GetComponent<GameObject>().SetActive(false);
        else if(obj.gameObject.tag.Equals("Block"))
        {
            Destroy(gameObject);
        }
    }
}
