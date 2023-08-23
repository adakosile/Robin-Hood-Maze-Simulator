using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject arrow;
    public GameObject enemies;
    public Animator anim;

    private Vector3 distanceToPlayer;
    private float speed;
    private bool flipX;
    private bool canShoot;
    private Vector3 enemyMvmtVector;
    
    public int health;

    void Start()
    {
        
        health = 3;  //TO BE IMPLMENETED
        canShoot = true;
        speed = 1.5f;
        enemyMvmtVector = 5f * Vector3.up;
    }
    
    private void Update()
    {
        //StartCoroutine(moveBackAndForth());
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 playerPosition = player.GetComponent<Transform>().position;
        float angle = Mathf.Atan2(playerPosition.y - transform.position.y, playerPosition.x - transform.position.x);
        //Vector3 enemyMvmtVector = new Vector3(speed * Mathf.Cos(angle), speed * Mathf.Sin(angle));
        Vector3 arrowAtk = new Vector3(3f * Mathf.Cos(angle), 3f * Mathf.Sin(angle));
        // Original Force Value: 1.6
        //convert to degrees
        angle = (180 / Mathf.PI) * angle;
        if (angle < 0f)
        {
            angle += 360;
        }

        if (angle > 90f && angle < 270f && !flipX)
        {
            flipX = true;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (angle <= 90f || angle >= 270f && flipX)
        {
            flipX = false;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        float maxDist = 4f;

        /*if (Vector3.Distance(player.GetComponent<Transform>().position, transform.position) >= maxDist)
        {
            transform.Translate(enemyMvmtVector*Time.deltaTime);
        }*/
        //if(Vector3.Distance(player.GetComponent<Transform>().position, transform.position) <= maxDist)
        if(IsVisibleToCamera(transform))
        {
            if(canShoot)
            {
                canShoot = false;
                StartCoroutine(ShootArrow(angle, arrowAtk));
            }
            
        }
        anim.Play("Run");
        transform.Translate(0.0f,speed*Time.deltaTime,0f);

    }
    IEnumerator moveBackAndForth()
    {
        while (true)
        {
            enemyMvmtVector = new Vector3(1.0f, 0.0f, 0.0f);
            yield return new WaitForSeconds(2);

            enemyMvmtVector = new Vector3(-1.0f, 0.0f, 0.0f);
            yield return new WaitForSeconds(2);

        }
    }
    //Debug.Log(angle);
    IEnumerator ShootArrow(float angle, Vector3 arrowAtk)
    {
        yield return new WaitForSeconds(.3f);
        GameObject enemyArr = Instantiate(arrow, transform.position, Quaternion.AngleAxis(angle, Vector3.forward)) as GameObject;
        /*
        Component[] enemyList = enemies.GetComponentsInChildren<Transform>();
        foreach (Component g in enemyList)
        {
            GameObject enemy = g.gameObject;
            Physics2D.IgnoreCollision(enemyArr.GetComponent<BoxCollider2D>(), enemy.GetComponent<BoxCollider2D>());

        }
        */
        Physics2D.IgnoreCollision(enemyArr.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        enemyArr.GetComponent<Rigidbody2D>().AddForce(arrowAtk);
        yield return new WaitForSeconds(4);   //delay between shots
        canShoot = true;
    }
    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.gameObject.tag.Equals("Block"))
        {
            
            if(transform.position.y < obj.gameObject.transform.position.y)
            {
                speed = -1.5f;
            }
            else if (transform.position.y > obj.gameObject.transform.position.y)
            {
                speed = 1.5f;
            }
        }
        if (obj.gameObject.tag.Equals("Player") &&!GetComponent<Collider2D>().IsTouching(obj.gameObject.GetComponent<PolygonCollider2D>()))
        {
            obj.gameObject.SendMessage("DecreaseHealth");
        }
    }
    public static bool IsVisibleToCamera(Transform trans)
    {
        Vector3 visTest = Camera.main.WorldToViewportPoint(trans.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1);
    }
}
