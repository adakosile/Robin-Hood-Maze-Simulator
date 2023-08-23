using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
        gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.gameObject.tag.Equals("Player") && obj.gameObject.GetComponent<Player>().collisionsEnabled)
        {
            if (obj.gameObject.GetComponent<PolygonCollider2D>() == null) {
                obj.gameObject.SendMessage("DecreaseHealth");
            }
            Destroy(gameObject);
        }
        else if (obj.gameObject.tag.Equals("Block"))
        {
            Destroy(gameObject);
        }
        else if (obj.gameObject.tag.Equals("Weapon"))
        {
            Destroy(gameObject);
        }
    }
}
