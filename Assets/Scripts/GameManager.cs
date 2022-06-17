using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreenCanvas;

    public List<GameObject> enemiesInWave = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    private KeyboardPlayerController keyboardPlayer;
    private PlayerMouseController mousePlayer;
    private TowerController towerControl;

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
    public TextMeshProUGUI nextWaveKeyboard;
    public TextMeshProUGUI nextWaveMouse;

    public bool keyboardReady = false;
    public bool mouseReady = false;

    // for temporary damage boost
    private bool explosiveArrowDone = false;
    private float startTime;

    public GameObject tutorial;

    public int discountMult = 15;
    public bool discount;

    // for tutorial
    public int tutorialStage = 1;
    /*public bool hasMoved = false;
    public bool hasShotArrow = false;
    public bool hasDrawn = false;
    public bool hasSwitchedK = false;
    public bool hasSwitchedW = false;
    public bool hasAimed = false;*/
    public bool hasFinished = false;
    //public TextMeshProUGUI tutorialText;
    public GameObject skipTutorialText;

    public bool discounted = false;
    public bool playersStunned = false; // for temporarily pausing players when out of health

    // SFX
    public AudioSource generalSFX;
    public AudioClip playerStun;
    public AudioClip gameOverSFX;
    public AudioClip outOfDrawBar;

    public bool canNoDrawBar;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("/Canvas/Tutorial").GetComponent<GameObject>().SetActive(true);
        //GameObject.Find("/Canvas/Tutorial 2").GetComponent<GameObject>().SetActive(true);
        drawText.text = "Draw: " + drawBarValue;
        playerHealthText.text = "Health: " + playerHealth;
        keyboardPlayer = GameObject.Find("KeyboardPlayer").GetComponent<KeyboardPlayerController>();
        mousePlayer = GameObject.Find("MousePlayer").GetComponent<PlayerMouseController>();
        towerControl = GameObject.Find("Tower").GetComponent<TowerController>();
        skipTutorialText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (wave == 0) { // tutorial
            switch (tutorialStage)
            {
                case 1: // display WASD tutorial
                    tutorial.transform.position = new Vector3(0, 1.06f, 0);
                    tutorial.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial1");
                    break;
                case 2: // display shooting tutorial
                    tutorial.transform.position = new Vector3(-5.5f, 1.06f, 0);
                    tutorial.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial2");
                    break;   
                case 3: // display drawing tutorial
                    tutorial.transform.position = new Vector3(5.5f, 1.06f, 0);
                    tutorial.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial3");   
                    break;    
                case 4: // display arrow switching tutorial
                    tutorial.transform.position = new Vector3(-5.29f, -0.82f, 0);
                    tutorial.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial4");
                    break;   
                case 5: // display wall switching tutorial
                    tutorial.transform.position = new Vector3(5.5f, 1.06f, 0);
                    tutorial.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial5");
                    break;   
                case 6: // display cooperative aiming tutorial
                    tutorial.transform.position = new Vector3(-5.5f, 1.06f, 0);
                    tutorial.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial6");
                    break;
                case 7: // display shop opening tutorial
                    tutorial.transform.position = new Vector3(0, 1.8f, 0);
                    tutorial.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial7");
                    break;
                case 8:
                    tutorial.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial8");
                    skipTutorialText.gameObject.transform.position = new Vector3(0, -4, 0);
                    break;
                case 9:
                    tutorial.transform.position = new Vector3(0, -2.6f, 0);
                    tutorial.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tutorial9");
                    skipTutorialText.SetActive(false);
                    break;

            }
        }
        else 
        {
            tutorial.SetActive(false);
            skipTutorialText.SetActive(false);
        }
        
        if (playerHealth <= 0 && !playersStunned) {
            StartCoroutine(StunPlayers());
            playersStunned = true;
            generalSFX.clip = playerStun;
            generalSFX.Play();
        }

        if (drawBarValue < 0) // resets draw bar if value dips below 0
        {
            drawBarValue = 0;
            adjustDrawBarValue(0);
        }

        waveCount.text = "Wave: " + wave;
        enemiesRemainingCount.text = "Enemies: " + enemiesRemaining;
        goldText.text = "Gold: " + money;

        if (Input.GetKeyDown(KeyCode.P)) // devtool to spawn in enemies
        {
            Vector3 spawnPos = new Vector3(10,0,-1);
            Quaternion spawnRotation = new Quaternion(0, 0, 0, 0);
            Instantiate(enemy, spawnPos, spawnRotation);
        }
        if (Input.GetKey(KeyCode.O)) // devtool to add more drawbar
        {
            adjustDrawBarValue(1000);
        }

        KeyboardReadyUp();

        if (drawBarValue == 0 && canNoDrawBar)
        {
            generalSFX.clip = outOfDrawBar;
            generalSFX.Play();
            canNoDrawBar = false;
        }

        if(drawBarValue > 0)
        {
            canNoDrawBar = true;
        }
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

    public void adjustMaxDraw(int num)
    {
        drawBarMaxValue += num;
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
        keyboardReady = false;
        mouseReady = false;
        nextWaveKeyboard.text = "NextWave\n(Enter key)";
        nextWaveMouse.text = "NextWave\n(Click)";
        towerControl.ShopReset();
        tutorial.SetActive(false);
        towerControl.mouseNextWave.SetActive(false);
        towerControl.keyboardNextWave.SetActive(false);
        discount = true;
        //GameObject.Find("/Canvas/Tutorial").GetComponent<GameObject>().SetActive(false);
        //GameObject.Find("/Canvas/Tutorial 2").GetComponent<GameObject>().SetActive(false);
        wave += 1;
        numEnemies = 0;
        currentWaveValue = 0;
        waveValueCap = (wave * ((wave - 1) / 2)) + 10;
        //Debug.Log("NewWave start");

        towerControl.addItem(wave);

        if(wave <= 10 && wave % 2 == 0 && maxEnemy != enemies.Count)
        {
            maxEnemy += 1;
        }

        while (currentWaveValue < waveValueCap)
        {
            GameObject nextEnemy = enemies[Random.Range(minEnemy, maxEnemy)];
            enemiesInWave.Add(nextEnemy);
            currentWaveValue += nextEnemy.GetComponent<Enemy>().value;
            numEnemies += nextEnemy.GetComponent<Enemy>().numOfEnemies;
        }
        //Debug.Log("NewWave end");

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
                spawnX = 10;
            } else if (side == 1)
            {
                spawnX = Random.Range(-12, 12);
                spawnY = 6;
            }
            else if (side == 2)
            {
                spawnY = Random.Range(-6, 6);
                spawnX = -10;
            }
            else if (side == 3)
            {
                spawnX = Random.Range(-12, 12);
                spawnY = -6;
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
        gameOverScreenCanvas.SetActive(true);
        generalSFX.clip = gameOverSFX;
        generalSFX.Play();
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
        SceneManager.LoadScene("Game");
    }

    public float TempDamageMonitor ()
    {
        if (explosiveArrowDone == false)
        {
            startTime = Time.deltaTime;
            explosiveArrowDone = true;
            return 1.5f;
        }
        else if (startTime + 10.0f >= Time.deltaTime)
        {
            return 1.5f;
        }

        return 1;
    }

    public void MaxHealthUpgrade()
    {
        if (money >= 25)
        {
            maxPlayerHealth += 10;
            playerHealth = maxPlayerHealth;
            UpdateMoney(-25);
        }
        

    }

    public void ButtonTest()
    {
        Debug.Log("Test");
    }

    public void MouseReadyUp()
    {
        if (!mouseReady)
        {
            mouseReady = true;
            nextWaveMouse.text = "Mouse Ready";
            if (keyboardReady)
            {
                NewWave();
            }
        }
        else
        {
            nextWaveMouse.text = "NextWave\n(Click)";
            mouseReady = false;
        }

    }

    public void KeyboardReadyUp()
    {
        if (Input.GetKeyDown(KeyCode.Return) && towerControl.keyboardNextWave)
        {
            if (!keyboardReady)
            {
                keyboardReady = true;
                nextWaveKeyboard.text = "Keyboard Ready";
                if (mouseReady)
                {
                    NewWave();
                }
            }
            else
            {
                nextWaveKeyboard.text = "NextWave\n(Enter key)";
                keyboardReady = false;
            }
        }
    }

     public IEnumerator StunPlayers()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime((float) 5 / maxPlayerHealth); // recovery time is always 5 seconds
        for (int i = 0; i <= maxPlayerHealth; i++)
        {
            //Debug.Log(Time.time);
            adjustHealthValue(1);
            yield return wait;
        }
        playersStunned = false;
    }
}
