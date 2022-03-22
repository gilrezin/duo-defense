using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TowerController : MonoBehaviour
{
    private GameManager gameManager;
    public float health = 100;
    public GameObject healthBar;
    public GameObject gameManagerObj;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
