using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject tower;
    private TowerController towerControl;
    private Vector3 towerLocation;
    public float speed;
    public float damage = 5;
    public float health = 5;
    public float pushbackForce;
    public int value = 1;
    public int resourceRefill;
    private GameManager gameManager;
    public List<GameObject> lootList;
    private bool hasDied = false;
    public int numOfEnemies;
    public AudioSource enemySFX;
    public AudioClip playerTakeDamage;
    public AudioClip enemyTakeDamage;
    public AudioClip enemyDeath;
    public AudioClip wallDamage;
    public GameObject hurtPlayerParticles;
    public GameObject hurtParticles;
    public GameObject damageWallParticles;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameOver())
        {
            Destroy(gameObject);
        }
    }
    
    public void DealDamage (float damage)
    {
        bool usedRearm = false;
        health = health - damage;
        Instantiate(hurtParticles, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            if (hasDied == false)
            {
                gameManager.enemiesRemaining -= 1;
                hasDied = true;
            }
            
            int r = Random.Range(1, 3);
            for (int i = 0; i < r; i++)
            {
                GameObject loot = lootList[Random.Range(0, lootList.Count)];
                if (!usedRearm && (loot.CompareTag("Explosive Arrow Drop") || loot.CompareTag("Multi Arrow Drop")))
                {
                    Instantiate(loot, gameObject.transform.position, gameObject.transform.rotation);
                    usedRearm = true;
                }
                else if (loot.CompareTag("Basic Health Jug"))
                {
                    Instantiate(loot, gameObject.transform.position, gameObject.transform.rotation);
                }
                else
                {
                    Instantiate(lootList[0], gameObject.transform.position, gameObject.transform.rotation);
                }
                
            }
            GameObject.Find("GameManager").GetComponent<GameManager>().adjustDrawBarValue(resourceRefill);
            
            Die();

        }
    }

    private void HurtPlayer() // method used for lowering player health bar, created to avoid duplication of code
    {
        gameManager.adjustHealthValue(-1);
        gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.down * pushbackForce * 0.5f);
        enemySFX.clip = enemyTakeDamage;
        enemySFX.Play();
        Instantiate(hurtPlayerParticles, transform.position, Quaternion.identity);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.CompareTag("Tower")) // enemy damages tower
        {
            hit.GetComponent<TowerController>().TakeDamage(damage);
            enemySFX.clip = wallDamage;
            enemySFX.Play();
            Instantiate(damageWallParticles, transform.position, Quaternion.identity);

            gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.down * pushbackForce);
        }
        else if (hit.CompareTag("Wall")) // Enemy breaks wall
        {
           gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.down * pushbackForce);
           enemySFX.clip = wallDamage;
           enemySFX.Play();
           Instantiate(damageWallParticles, transform.position, Quaternion.identity);
        }
        else if (collision.gameObject.CompareTag("Player")) // Keyboard player takes damage
        {
            HurtPlayer();
        }
    }

    public void Die()
    {
        enemySFX.clip = enemyDeath;
        enemySFX.Play();
        //Debug.Log("EnemyDestroyed");
        Destroy(gameObject);
    }

    void OnMouseOver() // Mouse player takes damage
    {
        HurtPlayer();
    }
}