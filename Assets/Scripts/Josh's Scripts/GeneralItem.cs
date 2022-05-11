using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralItem : MonoBehaviour
{

    private TowerController tower;
    private KeyboardPlayerController keyboardPlayer;
    private GameManager gameManage;
    public int itemIndex;
    private GameObject item;
    public int cost;

    // Start is called before the first frame update
    void Start()
    {
        tower = GameObject.Find("Tower").GetComponent<TowerController>();
        keyboardPlayer = GameObject.Find("KeyboardPlayer").GetComponent<KeyboardPlayerController>();
        gameManage = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tower.destroyItems == true)
        {
            Destroy(gameObject);
        } 

        if(tower.hideItems == true)
        {
            gameObject.SetActive(false);
        } 
        if (tower.hideItems == false)
        {
            gameObject.SetActive(true);
        }
    }

    public void OnBuy()
    {
        if (itemIndex == 0 && gameManage.money >= cost)
        {
            keyboardPlayer.numOfExplosiveArrows += 10;
            gameManage.money -= cost;
        } else if (itemIndex == 1)
        {
            //keyboardPlayer.bow = 
        }
    }

}
