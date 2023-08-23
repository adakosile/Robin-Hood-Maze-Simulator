using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponIndicator : MonoBehaviour
{
    public GameObject currentWeapon;

    public Sprite topHudSelect;
    public Sprite midHudSelect;
    public Sprite botHudSelect;
    public GameObject bow;
    public GameObject sword;
    public GameObject bomb;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        sword.SetActive(false);
        bomb.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            GetComponent<Image>().sprite = topHudSelect;
        }
        else if (sword.activeSelf && Input.GetKeyDown("2"))
        {
            GetComponent<Image>().sprite = midHudSelect;
        }
        else if(bomb.activeSelf && Input.GetKeyDown("3"))
        {
            GetComponent<Image>().sprite = botHudSelect;
        }
    }
    void setSwordActive()
    {
        sword.SetActive(true);
        GetComponent<Image>().sprite = midHudSelect;

    }
    void setBombActive()
    {
        bomb.SetActive(true);
        GetComponent<Image>().sprite = botHudSelect;
    }
}
