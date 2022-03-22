using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemiesInWave = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    public GameObject shop;

    private KeyboardPlayerController keyboardPlayer;
    private PlayerMouseController mousePlayer;

    public int money = 100000;
    private int playerHealth = 50; // starting health for the players (health bar is shared)
    private int maxPlayerHealth = 50; // maximum player health
    public int keyboardCost = 50;
    public int mouseCost = 50;

    public int currentWaveValue;
    public int wave = 0;
    public GameObject enemy;
    public TextMeshProUGUI drawText; // draw label shown in the draw bar
    public TextMeshProUGUI playerHealthText; // health label shown in the upper health bar
    public GameObject healthBar; // red bar displaying amount of player health left
    public GameObject drawBar; // blue bar displaying amount of drawing left
    private int drawBarValue = 100;
    public int drawBarMaxValue;    
    int numEnemies = 0;
    public int enemiesRemaining;
    public int waveValueCap = 10;
    int minEnemy = 0;
    int maxEnemy = 1;
    // for gameover
    public bool isGameOver = false;
    public TextMeshProUGUI gameOverScreen;
    public TextMeshProUGUI restartButton;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI waveCount;
    public TextMeshProUGUI enemiesRemainingCount;
    
    // Start is called before the first frame update
    void Start()
    {
        drawText.text = "Draw: " + drawBarValue;
        playerHealthText.text = "Health: " + playerHealth;
        keyboardPlayer = GameObject.Find("KeyboardPlayer").GetComponent<KeyboardPlayerController>();
        mousePlayer = GameObject.Find("MousePlayer").GetComponent<PlayerMouseController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesRemaining == 0)
        {
            shop.SetActive(true);
            
        }

        else if (playerHealth <= 0) {
            GameOver();
        }

        waveCount.text = "Wave: " + wave;
        enemiesRemainingCount.text = "Enemies Remaining: " + enemiesRemaining;
        goldText.text = "Gold: " + money;
    }

    public void adjustDrawBarValue(int value) { // modifies the draw bar value by a given amount
        drawBarValue += value;
        if (drawBarValue >= drawBarMaxValue)
        {
            drawBarValue = drawBarMaxValue;
        }
        drawText.text = "Draw: " + drawBarValue;
        drawBar.transform.localScale = new Vector3((float) drawBarValue / (float) drawBarMaxValue * 450, 7.5f, 1);
    }

    public int getDrawBarValue() {
        return drawBarValue;
    }

    public void adjustHealthValue(int value) { // modifies the player health value by a given amount
        playerHealth += value;
        if (playerHealth >= maxPlayerHealth) // player health cannot go above max player health
        {
            playerHealth = maxPlayerHealth;
        }
        else if (playerHealth < 0) 
        {
            playerHealth = 0;
        }
        playerHealthText.text = "Health: " + playerHealth;
        healthBar.transform.localScale = new Vector3((float) playerHealth / (float) maxPlayerHealth * 640, 7.5f, 1);
    }

    public int getHealthValue() { // gets the current player health
        return playerHealth;
    }

    // creates a list of enemeis to be spawned in the next wave
    public void NewWave()
    {
        shop.SetActive(false);
        wave += 1;
        numEnemies = 0;
        currentWaveValue = 0;
        waveValueCap = (wave * ((wave - 1) / 2)) + 10;
        Debug.Log("NewWave start");
        
        if(wave <= 10 && wave % 2 == 0 && maxEnemy != enemies.Count)
        {
            maxEnemy += 1;
        }

        while (currentWaveValue < waveValueCap)
        {
            GameObject nextEnemy = enemies[Random.Range(minEnemy, maxEnemy)];
            enemiesInWave.Add(nextEnemy);
            currentWaveValue += nextEnemy.GetComponent<Enemy>().value;
            numEnemies += 1;
        }
        Debug.Log("NewWave end");

        StartCoroutine(SpawnEnemyInWave());
    }


// spawns enemy after a delay
    IEnumerator SpawnEnemyInWave()
    {

        enemiesRemaining = enemiesInWave.Count;

        for (int i = 0; i < enemiesInWave.Count; i++)
        {
            int side;
            float spawnX = 0;
            float spawnY = 0;
            if (wave <= 4)
            {
                side = Random.Range(0, wave);
            } else
            {
                side = Random.Range(0, 4);
            }
            
            if (side == 0)
            {
                spawnY = Random.Range(-6, 6);
                spawnX = 13;
            } else if (side == 1)
            {
                spawnX = Random.Range(-12, 12);
                spawnY = 7;
            }
            else if (side == 2)
            {
                spawnY = Random.Range(-6, 6);
                spawnX = -13;
            }
            else if (side == 3)
            {
                spawnX = Random.Range(-12, 12);
                spawnY = -7;
            }
            
            
            Vector3 spawnPos = new Vector3(spawnX, spawnY, -1);
            Quaternion spawnRotation = new Quaternion(0, 0, 0, 0);
            yield return new WaitForSeconds(2);
            Instantiate(enemiesInWave[i], spawnPos, spawnRotation);
            
        }

        enemiesInWave.Clear();

    }

    // does game over
    public void GameOver()
    {
        isGameOver = true;

    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void UpgradeMouse()
    {
        if (money >= mouseCost)
        {
            money -= mouseCost;
            drawBarMaxValue += 25;

        }
        
    }

    public void UpgradeKeyboard()
    {
        if (money >= keyboardCost)
        {
            money -= keyboardCost;
            keyboardPlayer.damageX += 0.2f;
        }
        
    }

    public void UpdateMoney(int add)
    {
        money += add;
    }


    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
