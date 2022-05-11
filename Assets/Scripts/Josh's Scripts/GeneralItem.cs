using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralItem : MonoBehaviour
{

    private TowerController tower;
    private KeyboardPlayerController keyboardPlayer;
    private GameManager gameManage;

    public int shelfNum;

    public bool kItem;
    public bool mItem;

    public string toolTip;
    public float textSize;

    public string title;

    public bool discount;
    bool discounted;
    public int discountMult = 2;

    public int itemIndex;
    private GameObject item;
    public int cost;
    public int originalCost;
    private bool inPosK = true;
    private bool inPosM = true;
    // Start is called before the first frame update
    void Start()
    {
        tower = GameObject.Find("Tower").GetComponent<TowerController>();
        keyboardPlayer = GameObject.Find("KeyboardPlayer").GetComponent<KeyboardPlayerController>();
        gameManage = GameObject.Find("GameManager").GetComponent<GameManager>();
        originalCost = cost;
    }

    // Update is called once per frame
    void Update()
    {
        if (tower.DestroyItems == true)
        {
            Destroy(gameObject);
        }

        if (tower.hideItemsK == true && kItem && inPosK)
        {
            gameObject.transform.Translate(100, 0, 0);
            inPosK = false;
        } else if (tower.hideItemsK == false && kItem && !inPosK)
        {
            gameObject.transform.Translate(-100, 0, 0);
            inPosK = true;
        }

        if (tower.hideItemsM == true && mItem && inPosM)
        {
            gameObject.transform.Translate(100, 0, 0);
            inPosM = false;
        }
        else if (tower.hideItemsM == false && mItem && !inPosM)
        {
            gameObject.transform.Translate(-100, 0, 0);
            inPosM = true;
        }

        if (gameManage.enemiesRemaining == 0)
        {
            if (!discounted)
            {
                discounted = false;
                discount = false;
                cost *= 3 / discountMult;
            }
            
        } else
        {
            discount = true;
            discounted = true;
            cost = originalCost;
        }

    }

    public void OnBuy()
    {
        if ( gameManage.money >= cost)
        {

            gameManage.money -= cost;

            if (kItem)
            {
                if (itemIndex == 0)
                {
                        keyboardPlayer.numOfExplosiveArrows += 10;
                }
                else if (itemIndex == 1)
                {
                    keyboardPlayer.bow = GameObject.Find("/Bows/ShortBow");
                }
            }
            else if (mItem)
            {
                if (itemIndex == 0)
                {
                    gameManage.adjustMaxDraw(20);
                    gameManage.adjustDrawBarValue(40);
                }
                else if (itemIndex == 1)
                {
                    gameManage.adjustDrawBarValue(40);
                }

            }
        }
        else
        {
            tower.CantAfford(shelfNum, kItem);
        }

    }

}
