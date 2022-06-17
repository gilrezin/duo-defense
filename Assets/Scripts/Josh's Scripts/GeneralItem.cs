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

    public bool consumable;

    public string toolTip;
    public float textSize;

    public string title;

    public bool discounted;
    public int discountMult = 30;

    public int itemIndex;
    private GameObject item;
    public int cost;
    public int originalCost;
    private bool inPosK = true;
    private bool inPosM = true;

    //SFX
    public AudioSource itemSFX;
    public AudioClip buyItem;
    public AudioClip cantAffordSFX;
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

        if (gameManage.discount && !discounted)
        {
            Discount();
        }

         if (gameManage.enemiesRemaining == 0)
        {
            discounted = false;
            cost = originalCost;
        }

    }

    public void Discount()
    {
        cost *= 4;
        cost /= 5;
        discounted = true;
    }

    public void OnBuy()
    {
        if ( gameManage.money >= cost)
        {
            itemSFX.clip = buyItem;
            itemSFX.Play();
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
                    for (int k = 0; k < tower.kItems.Count; k++)
                    {
                        //Debug.Log("check: " + k);
                        if (tower.kItems[0] == GameObject.Find("/Bows/ShortBow"))
                        {
                            tower.kItems.RemoveAt(k);
                            //Debug.Log("Remove");
                        }
                    }

                }
                else if (itemIndex == 2)
                {
                    keyboardPlayer.numOfMultiArrows += 10;
                }
            }
            else if (mItem)
            {
                if (itemIndex == 0)
                {
                    gameManage.adjustMaxDraw(20);
                    gameManage.adjustDrawBarValue(20);
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
            itemSFX.clip = cantAffordSFX;
            itemSFX.Play();
        }

    }

}
