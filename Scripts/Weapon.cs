using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject player;
    public string weaponName;
    public bool weaponSwitch;

    public Sprite bowSprite;
    public Sprite swordSprite;

    private bool lookRight;
    private bool lookLeft;
    private bool lookDown;
    private bool lookUp;
    private bool isAttacking; //true when the weapon is calling animations
    // Start is called before the first frame update
    void Start()
    {
        
        weaponSwitch = false;
        lookRight = true;
        lookLeft = false;
        lookDown = false;
        lookUp = false;
        weaponName = "bow";
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponSwitch)
        {
            if(weaponName.Equals("bow"))
            {
                GetComponent<SpriteRenderer>().sprite = bowSprite;
                GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            else if(weaponName.Equals("sword"))
            {
                GetComponent<RectTransform>().localScale = new Vector3(1.3f, 1.3f, 1);
                GetComponent<SpriteRenderer>().sprite = swordSprite;
            }
            weaponSwitch = false;
        }
    }

    void UpdateWeaponAngle(float angle)
    {
        //set sort order to 1 to be behind the player model
        //set sort order to 3 or 4 to be above the player model
        //top left goes to about 157.3
        //bottom left goes to about 202.8
        //top right is about 22.5
        //337.2 for bottom right
        
        Vector3 playerPosition = player.GetComponent<Transform>().position;
        if (weaponName.Equals("bow"))
        {
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            GetComponent<SpriteRenderer>().flipX = false;
            //set sort order to 1 to be behind the player model
            //set sort order to 3 or 4 to be above the player model
            //top left goes to about 157.3
            //bottom left goes to about 202.8
            //top right is about 22.5
            //337.2 for bottom right
            if (angle < 21.5 || angle > 337.2 && !lookRight)
            {
                lookRight = true;
                lookUp = false;
                lookLeft = false;
                lookDown = false;
                transform.position = new Vector3(playerPosition.x + .03f, playerPosition.y, 0);
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (angle > 21.5 && angle < 157.3 && !lookUp)
            {
                lookRight = false;
                lookUp = true;
                lookLeft = false;
                lookDown = false;
                transform.position = new Vector3(playerPosition.x, playerPosition.y + .03f, 0);
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (angle > 157.3 && angle < 202.8 && !lookLeft)
            {
                lookRight = false;
                lookUp = false;
                lookLeft = true;
                lookDown = false;
                transform.position = new Vector3(playerPosition.x - .03f, playerPosition.y, 0);
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (angle > 202.8 && angle < 337.2 && !lookDown)
            {
                lookRight = false;
                lookUp = false;
                lookLeft = false;
                lookDown = true;
                transform.position = new Vector3(playerPosition.x, playerPosition.y - .03f, 0);
                GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
        }
        else if (weaponName.Equals("sword") && !isAttacking)
        {
            if (angle < 21.5 || angle > 337.2 && !lookRight)
            {
                lookRight = true;
                lookUp = false;
                lookLeft = false;
                lookDown = false;
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            // Original: angle < 157.3
            if (angle > 21.5 && angle < 90 && !lookUp)
            {
                lookRight = false;
                lookUp = true;
                lookLeft = false;
                lookDown = false;
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (angle > 90 && angle < 202.8 && !lookLeft)
            {
                lookRight = false;
                lookUp = false;
                lookLeft = true;
                lookDown = false;
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (angle > 202.8 && angle < 337.2 && !lookDown)
            {
                lookRight = false;
                lookUp = false;
                lookLeft = false;
                lookDown = true;
                GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
            if (lookLeft)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                transform.position = new Vector3(playerPosition.x - .22f, playerPosition.y, 0);
                GetComponent<SpriteRenderer>().flipX = true;
                GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
            else if(lookRight)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                transform.position = new Vector3(playerPosition.x + .22f, playerPosition.y, 0);
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
            else if(lookUp)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                transform.position = new Vector3(playerPosition.x + .22f, playerPosition.y, 0);
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            else if(lookDown)
            {
                if(angle > 270)
                {
                    transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                    transform.position = new Vector3(playerPosition.x + .22f, playerPosition.y, 0);
                    GetComponent<SpriteRenderer>().flipX = false;
                    GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
                else
                {
                    transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                    transform.position = new Vector3(playerPosition.x - .22f, playerPosition.y, 0);
                    GetComponent<SpriteRenderer>().flipX = true;
                    GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
            }
        }
    }

    void SwordAttack()  //Swings the sword through a coroutine which is called
    {
        isAttacking = true;
        StartCoroutine(SwordSwingAnimation());
    }

    void BoomerangAttack(Vector3 mousePos)
    {
        isAttacking = true;
        StartCoroutine(SwordSwingAnimation());
    }

    IEnumerator SwordSwingAnimation()  //currently swings around the player like a spin attack lmao
    {
        if (GetComponent<SpriteRenderer>().flipX == false)
        {
            float angle = 6;
            Vector3 playerPosition = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y - .01f, 1);
            for (int i = 0; i < 15; i++)
            {
                playerPosition = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y - .2f, 1);
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.RotateAround(playerPosition, Vector3.forward, -1 * angle);
                yield return new WaitForSeconds(.0001f);
            }
            for (int i = 0; i < 15; i++)
            {
                playerPosition = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y - .2f, 1);
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.RotateAround(playerPosition, Vector3.forward, angle);
                yield return new WaitForSeconds(.0001f);
            }
        }
        else
        {
            float angle = 6;
            Vector3 playerPosition = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y - .01f, 1);
            for (int i = 0; i < 15; i++)
            {
                playerPosition = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y - .2f, 1);
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.RotateAround(playerPosition, Vector3.forward, angle);
                yield return new WaitForSeconds(.0001f);
            }
            for (int i = 0; i < 15; i++)
            {
                playerPosition = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y - .2f, 1);
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.RotateAround(playerPosition, Vector3.forward, -1 * angle);
                yield return new WaitForSeconds(.0001f);
            }
        }
        isAttacking = false;
    }

    IEnumerator BoomerangSwordAnimation(Vector3 mousePosition)  //TO BE IMPLEMENTED
    {
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator SwordSpinAnimation()  //TO BE IMPLEMENTED
    {
        yield return new WaitForSeconds(.5f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(weaponName.Equals("sword") && isAttacking && col.gameObject.tag.Equals("Enemy"))
        {
            Destroy(col.gameObject);  //CHANGE THIS TO LOWER HEALTH 
        }
    }
    void DecreaseHealth()
    {

    }
}


// Use your spin attack accident.
// Use your boomerang accident
// Implement bombs