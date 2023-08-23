using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int arrowammo;
    public int bombammo;
    public int health;

    public Sprite halfHeart;
    public Sprite emptyHeart;
    
    public Animator anim;
    public Camera camera;
    public float speed;
    public GameObject hearts;
    public GameObject currentWeapon;
    public GameObject arrow;
    public GameObject pauseMenu;
    public GameObject weaponIndicator;
    public Text ammoText;
    public bool collisionsEnabled;  //SET TO FALSE ONLY WHEN ROLLING
    public AudioClip walkingSound;
    public AudioClip swingSwordSound;
    public AudioClip rollSound;
    AudioSource audioSource;
    public GameObject[] enemies;

    private bool canShoot;  //set to false when killed
    private bool canRoll; //set to true when character can roll, false when player cannot roll due to timer or death
    private bool isPaused;
    private bool resumeGame;
    private bool isAttacking; //used for collisions - checks if 
    private bool isRolling; //true if the character is rolling - CHANGED ONLY IF HE IS TRULY ROLLING
    private bool swordEquipped;  //TRUE IF SWORD IS EQUIPPED AFTER PICKING IT UP
    private bool bombEquipped;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        audioSource = GetComponent<AudioSource>();

        swordEquipped = false;
        bombEquipped = false;
        collisionsEnabled = true;
        canRoll = true;
        isRolling = false;
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;

        canShoot = true;
        health = 6;
        arrowammo = 15;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        ammoText.text = "Enemies Left: "+enemies.Length;
        currentWeapon.GetComponent<Weapon>().weaponName = "bow";
        Physics2D.IgnoreCollision(currentWeapon.GetComponent<PolygonCollider2D>(), GetComponent<BoxCollider2D>());
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown("escape") && !isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseMenu.SetActive(true);
        }
        else if ((Input.GetKeyDown("escape") && isPaused) || resumeGame)
        {
            //Debug.Log("Go back to the game!");
            Time.timeScale = 1;
            isPaused = false;
            pauseMenu.SetActive(false);
            resumeGame = false;
        }
        

        //Trying to pick up objects
        var objects = GameObject.FindGameObjectsWithTag("PickupObject");
        //Debug.Log(objects.Length);
        foreach (var obj in objects)
        {
            if(Vector2.Distance(transform.position, obj.transform.position) <= .3f && 
                obj.gameObject.name.Equals("SwordPickup") && Input.GetKeyDown("e"))
            {
                //Debug.Log("item detected");
                currentWeapon.GetComponent<Weapon>().weaponName = "sword";
                currentWeapon.GetComponent<Weapon>().weaponSwitch = true;
                swordEquipped = true;
                weaponIndicator.SendMessage("setSwordActive");
                obj.gameObject.SetActive(false);
            }
        }
        if (health > 0 && Input.GetKeyDown("1"))
        {
            currentWeapon.GetComponent<Weapon>().weaponName = "bow";
            currentWeapon.GetComponent<Weapon>().weaponSwitch = true;
            currentWeapon.GetComponent<Weapon>().SendMessage("updateWeaponAngle", 0);
            // FIX BOW ANGLE
        }
        if (health > 0 && Input.GetKeyDown("2") && swordEquipped)
        {
            currentWeapon.GetComponent<Weapon>().weaponName = "sword";
            currentWeapon.GetComponent<Weapon>().weaponSwitch = true;
        }

        //Use vector distance to find weapons in the area
    }
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        ammoText.text = "Enemies Left: " + enemies.Length;
        if(enemies.Length==0)
        {
            SceneManager.LoadScene("GameCompleted", LoadSceneMode.Single);
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if(health > 0)
        {
            if((horizontal > 0 || vertical > 0) && !audioSource.isPlaying)
            {
                audioSource.volume = .05f;
                audioSource.clip = walkingSound;
                audioSource.loop = true;
                audioSource.Play();
            }
            else if(((horizontal == 0 && vertical == 0) || health <= 0) && audioSource.isPlaying)
            {
                audioSource.volume = .05f;
                audioSource.clip = walkingSound;
                audioSource.Stop();
                audioSource.loop = false;
            }
            transform.Translate(horizontal * Time.deltaTime * speed, vertical * Time.deltaTime * speed, 0f);
        }

        // Get distance from camera to object
        float camDis = camera.transform.position.y-
            transform.position.y;
        // Get mouse position in world space. Use camDis for Z axis
        Vector3 mouse = camera.ScreenToWorldPoint
            (new Vector3(Input.mousePosition.x, 
            Input.mousePosition.y, camDis));
        


        float angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x);
        Vector3 arrowforce = new Vector3(2.3f * Mathf.Cos(angle), 2.3f * Mathf.Sin(angle));
       
        angle = (180 / Mathf.PI) * angle;
        

        if (angle <0f)
        {
            angle += 360;
        }
        float mouseAngle = angle;
        if (health > 0)
        {
            currentWeapon.SendMessage("UpdateWeaponAngle", angle);
        }
        angle = angle / 360.0f;
        if (Input.GetMouseButtonDown(0) && currentWeapon.GetComponent<Weapon>().weaponName.Equals("bow")){
            //Debug.Log("attack with weapon here?");
            if (canShoot && arrowammo != 0 && health != 0 && !isRolling) {
                //arrowammo--;
                canShoot = false;
                GameObject arr = Instantiate(arrow, transform.position, Quaternion.AngleAxis(mouseAngle, Vector3.forward)) as GameObject;   //Need to fix object position
                Physics2D.IgnoreCollision(arr.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
                Physics2D.IgnoreCollision(arr.GetComponent<BoxCollider2D>(), currentWeapon.GetComponent<PolygonCollider2D>());
                arr.GetComponent<Arrow>().spawnOrigin = gameObject;
                arr.GetComponent<Rigidbody2D>().AddForce(arrowforce);
                
                StartCoroutine(AttackTimer(2));   //allows you to shoot after whatever seconds
            }
        }

        if(Input.GetMouseButtonDown(0) && currentWeapon.GetComponent<Weapon>().weaponName.Equals("sword"))
        {
            if(canShoot && health != 0 && !isRolling)
            {
                audioSource.volume = .2f;
                audioSource.clip = swingSwordSound;
                audioSource.loop = false;
                audioSource.Play();
                canShoot = false;
                currentWeapon.SendMessage("SwordAttack");
                StartCoroutine(AttackTimer(.7f));
            }
        }

        bool isMoving = (horizontal != 0 || vertical != 0);
        bool rollPressed = Input.GetKeyDown(KeyCode.Space);
        //anim.SetBool("isMoving",isMoving);
        anim.SetFloat("angle", angle);
        //anim.SetBool("isRolling", isRolling);
        if(!isMoving && !isRolling)
        {
            anim.Play("Idle");
        }
        if(isMoving && !isRolling)
        {
            anim.Play("Run");
        }
        if(rollPressed && canRoll && health > 0)
        {
            canRoll = false;
            isRolling = true;
            Vector2 endPosition = new Vector2(transform.position.x,transform.position.y) + new Vector2(3 * Mathf.Cos(Mathf.Deg2Rad * angle*360.0f),
                3 * Mathf.Sin(Mathf.Deg2Rad * angle*360.0f));
            audioSource.volume = .2f;
            audioSource.clip = rollSound;
            audioSource.loop = false;
            audioSource.Play();
            StartCoroutine(rollTo(transform, endPosition));
        }
        //Debug.Log("" + angle);
        
    }

    /*void UpdateWeaponSelect()
    {
        if(Input.GetKeyDown("1"))
        {
            currentWeapon.GetComponent<Weapon>().weaponName = "bow";
            currentWeapon.GetComponent<Weapon>().weaponSwitch = true;
        }
        if (Input.GetKeyDown("2") && swordEquipped)
        {
            currentWeapon.GetComponent<Weapon>().weaponName = "sword";
            currentWeapon.GetComponent<Weapon>().weaponSwitch = true;
        }
    
    }*/

    void DecreaseHealth()
    {
    
        if (!isRolling) {
            Component[] heartList = hearts.GetComponentsInChildren<Transform>();
            health--;
            if (health == 5)
            {
                for (int i = 0; i < heartList.Length; i++)
                {
                    if (heartList[i].gameObject.name == "Heart3")
                    {
                        heartList[i].gameObject.GetComponent<Image>().sprite = halfHeart;
                    }
                }
            }
            else if (health == 4)
            {
                for (int i = 0; i < heartList.Length; i++)
                {
                    if (heartList[i].gameObject.name == "Heart3")
                    {
                        heartList[i].gameObject.GetComponent<Image>().sprite = emptyHeart;
                    }
                }
            }
            else if (health == 3)
            {
                for (int i = 0; i < heartList.Length; i++)
                {
                    if (heartList[i].gameObject.name == "Heart2")
                    {
                        heartList[i].gameObject.GetComponent<Image>().sprite = halfHeart;
                    }
                }
            }
            else if (health == 2)
            {
                for (int i = 0; i < heartList.Length; i++)
                {
                    if (heartList[i].gameObject.name == "Heart2")
                    {
                        heartList[i].gameObject.GetComponent<Image>().sprite = emptyHeart;
                    }
                }
            }
            else if (health == 1)
            {
                for (int i = 0; i < heartList.Length; i++)
                {
                    if (heartList[i].gameObject.name == "Heart1")
                    {
                        heartList[i].gameObject.GetComponent<Image>().sprite = halfHeart;
                    }
                }
            }
            else if (health == 0)
            {
                for (int i = 0; i < heartList.Length; i++)
                {
                    if (heartList[i].gameObject.name == "Heart1")
                    {
                        heartList[i].gameObject.GetComponent<Image>().sprite = emptyHeart;
                        canShoot = false;
                        StartCoroutine(PlayerKilled());
                    }
                }

            }
        }
    }

    void ResumeGame()
    {
        resumeGame = true;
    }

    void SetAttackState(bool newAtkState)
    {

    }

    IEnumerator PlayerKilled()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    IEnumerator AttackTimer(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        canShoot = true;
    }

    IEnumerator rollTo(Transform initialPosition, Vector2 toPosition)
    {
        collisionsEnabled = false;
        currentWeapon.SetActive(false);
        float counter = 0;
        int x = 0;
        Vector2 startPos = initialPosition.position;
        anim.Play("Roll");
        float duration = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        Debug.Log(duration);
        while(x < 10)
        {
            x++;
            initialPosition.position = Vector2.Lerp(initialPosition.position, toPosition, .1f);
            yield return new WaitForSeconds(.03f);
        }
        isRolling = false;
        currentWeapon.SetActive(true);
        collisionsEnabled = true;
        yield return new WaitForSeconds(5);    //CANNOT ROLL FOR 5 SECONDS AFTER ROLLING, change when needed
        canRoll = true;
    }


}
