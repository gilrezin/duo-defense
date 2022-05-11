
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TowerController : MonoBehaviour
{
    private GameManager gameManager;
    private KeyboardPlayerController keyboardPlayer;

    public float health = 100;
    public GameObject healthBar;

    public GameObject gameManagerObj;
    public GameObject keyboardShop;
    public bool kShop = false;
    public GameObject mouseShop;
    public bool mShop = false;
    public GameObject[,] keyboardItems;
    public int shopSize = 2;

    public int kItemsCap;
    public int MItemsCap;

    public List<GameObject> kItems = new List<GameObject>();
    public List<GameObject> mItems = new List<GameObject>();

    public GameObject[,] keyboardShelves = new GameObject[2, 2];

    public GameObject keyboardItem;

    private bool canReset = true;
    public bool destroyItems = false;
    public bool hideItems = true;
    // Start is called before the first frame update
    void Start()
    {
        ShopReset();
        gameManager = gameManagerObj.GetComponent<GameManager>();
        keyboardPlayer = GameObject.Find("KeyboardPlayer").GetComponent<KeyboardPlayerController>();
        keyboardItems = new GameObject[shopSize, shopSize];

        keyboardShelves[0, 0] = GameObject.Find("/Shop/KeyboardShop/ItemShelf");
        keyboardShelves[0, 1] = GameObject.Find("/Shop/KeyboardShop/ItemShelf 2");
        keyboardShelves[1, 0] = GameObject.Find("/Shop/KeyboardShop/ItemShelf 3");
        keyboardShelves[1, 1] = GameObject.Find("/Shop/KeyboardShop/ItemShelf 4");

        keyboardShop.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2) && !kShop)
        {
            destroyItems = false;
            hideItems = false;
            keyboardShop.SetActive(true);
            kShop = true;
        } else if (Input.GetMouseButtonDown(2) && kShop)
        {
            hideItems = true;
            keyboardShop.SetActive(false);
            kShop = false;
        }

        if (Input.GetKey(KeyCode.E) && !mShop)
        {
            mouseShop.SetActive(true);
            mShop = true;
        } else if (Input.GetKey(KeyCode.E) && mShop)
        {
            mouseShop.SetActive(false);
            mShop = false;
        }

        if (Input.GetKey(KeyCode.R) && canReset)
        {
            ShopReset();
            canReset = false;
        }
        if (Input.GetKey(KeyCode.M) && !canReset)
        {
            canReset = true;
        }

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.transform.localScale = new Vector3(health, 7.5f, 1);
        if (health <= 0)
        {
            gameManager.GameOver();
        }
    }

    public Vector3 FindPosition()
    {
        return transform.position;
    }
    /*public void DealDamage(float damage)
    {
        health = health - damage;
        /*if (health < 0) {
        endgame();
        */
    //}

    public void ShopReset()
    {
        ShopClear();
        StartCoroutine(ShopFill());
    }

    private void ShopClear()
    {
        destroyItems = true;
    }

    // spawns items on shop shelves
    IEnumerator ShopFill()
    {
        
        yield return new WaitForSeconds(0.01f);

        destroyItems = false;

        for (int i = 0; i < kItems.Count; i++)
        {
            for (int k = 0; k < kItems.Count; k++)
            {
                // picks random item
                keyboardItems[i, k] = kItems[Random.Range(0, kItemsCap)];
                // sets spawn position
                int spawnX = i + 1;
                int spawnY = k;
                Vector3 spawnPos = new Vector3(spawnX, spawnY, 2);
                Quaternion spawnRot = new Quaternion(0, 0, 0, 0);

                // spawns item at position
                GameObject nextItem = Instantiate(keyboardItems[i, k], spawnPos, spawnRot);

                // gives shelf reference to item

                keyboardShelves[i, k].GetComponent<KeyboardShelf>().item = nextItem;
                keyboardShelves[i, k].GetComponent<KeyboardShelf>().setScript();

            }
        }
    }

}


