using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardPlayerController : MonoBehaviour
{
    public TextMeshProUGUI arrowCounter;
    public float dashSpeed;
    public float speed;
    public GameObject arrow;
    //private float nextDash = 0;
    public float cooldown;
    private GameManager gameManager;
    public GameObject arrow2;
    public GameObject arrow3;
    private int equippedArrow = 1;
    public int numOfExplosiveArrows = 5;
    // damage muliplier
    public float damageX;
    // currrent bow the playe has
    public GameObject bow;
    private Bow bowStats;
    public bool canShoot = true;
    private bool outOfAmmoTextActive = false;
    private GeneralArrow arrowStats;
    public int numOfMultiArrows = 5;
    public TextMeshProUGUI multiArrowCounter;
    public GameObject basicSelected;
    public GameObject explosiveSelected;
    public GameObject multiSelected;
    public Camera mainCamera;
    public TextMeshProUGUI outOfAmmoText; // text that appears when right clicking to change wall type

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        bowStats = bow.GetComponent<Bow>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        numOfExplosiveArrows = 5;
        numOfMultiArrows = 5;
        animator = GameObject.Find("KeyboardPlayer/player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // updates ui
        multiArrowCounter.text = numOfMultiArrows.ToString();
        arrowCounter.text = numOfExplosiveArrows.ToString();
        if (outOfAmmoTextActive)
        {
            outOfAmmoText.transform.position = transform.position;
        } else
        {
            outOfAmmoText.transform.position = new Vector2(500,500);
        }
        // gets arrow and arrow stats
        GameObject usedArrow = null;
        if (equippedArrow == 1 || Input.GetKeyDown(KeyCode.Alpha1))
        {
            usedArrow = arrow;
            equippedArrow = 1;
            basicSelected.SetActive(true);
            explosiveSelected.gameObject.SetActive(false);
            multiSelected.gameObject.SetActive(false);
        }
        if (equippedArrow == 2 || Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameManager.hasSwitchedK = true;
            usedArrow = arrow2;
            equippedArrow = 2;
            basicSelected.gameObject.SetActive(false);
            explosiveSelected.gameObject.SetActive(true);
            multiSelected.gameObject.SetActive(false);
        }
        if (equippedArrow == 3 || Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameManager.hasSwitchedK = true;
            usedArrow = arrow3;
            equippedArrow = 3;
           basicSelected.gameObject.SetActive(false);
            explosiveSelected.gameObject.SetActive(false);
            multiSelected.gameObject.SetActive(true);
        }
        if(!gameManager.IsGameOver())
        {
            if (Input.GetKeyDown(KeyCode.Space) && canShoot)
            {
                //Debug.Log("shoot");
                Vector3 location = transform.position;
                Quaternion rotation = transform.rotation;
                if (usedArrow == arrow || (usedArrow == arrow2 && numOfExplosiveArrows > 0) || (usedArrow == arrow3 && numOfMultiArrows > 0))
                {
                    if (usedArrow == arrow2)
                    {
                        numOfExplosiveArrows--;
                        arrowCounter.text = numOfExplosiveArrows.ToString();
                        //arrowCounter.text = numOfExplosiveArrows.ToString();
                    }
                    if (usedArrow == arrow3)
                    {
                        numOfMultiArrows--;
                        multiArrowCounter.text = numOfMultiArrows.ToString();
                    }
                    Instantiate(usedArrow, location, rotation);
                    if (animator.GetInteger("AnimationState") == 2 || animator.GetInteger("AnimationState") == 4 || animator.GetInteger("AnimationState") == 6)
                    {
                        animator.SetInteger("AnimationState", (int)2);
                    }
                    else
                    {
                        animator.SetInteger("AnimationState", (int)1);
                    }
                    canShoot = false;
                    StartCoroutine(shotCooldown());
                    /*if (!arrowStats.infinite)
                    {
                        arrowStats.remaining -= 1;
                    }*/
                }
                else if ((usedArrow == arrow2 && numOfExplosiveArrows == 0) || (usedArrow == arrow3 && numOfMultiArrows == 0)) // when out of ammo, display out of ammo text
                {
                    StartCoroutine(displayOutOfAmmoText());
                }
                
            }
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    gameManager.hasMoved = true;
                    if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
                    {
                        rotateAndMove(45);
                    } else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
                    {
                        rotateAndMove(315);
                    } else
                    {
                        rotateAndMove(0);
                    }
                    if (animator.GetInteger("AnimationState") != 1 && animator.GetInteger("AnimationState") != 2)
                    {
                        animator.SetInteger("AnimationState", (int)4);
                    }
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    gameManager.hasMoved = true;
                    if (!Input.GetKey(KeyCode.S))
                    {
                        rotateAndMove(270);
                    }    
                    if (Input.GetKey(KeyCode.S))
                    {
                        rotateAndMove(225);
                    }
                    if (animator.GetInteger("AnimationState") != 1 && animator.GetInteger("AnimationState") != 2)
                    {
                        animator.SetInteger("AnimationState", (int)4);
                    }
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    gameManager.hasMoved = true;
                    if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
                    {
                        rotateAndMove(135);
                    }
                    else
                    {
                        rotateAndMove(180);
                    }
                    if (animator.GetInteger("AnimationState") != 1 && animator.GetInteger("AnimationState") != 2)
                    {
                        animator.SetInteger("AnimationState", (int)3);
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    gameManager.hasMoved = true;
                    rotateAndMove(90);
                    if (animator.GetInteger("AnimationState") != 1 && animator.GetInteger("AnimationState") != 2)
                    {
                        animator.SetInteger("AnimationState", (int)3);
                    }
                    
                }

                else
                {
                    if (animator.GetInteger("AnimationState") != 1 && animator.GetInteger("AnimationState") != 2)
                    {
                        if (animator.GetInteger("AnimationState") == 3 || animator.GetInteger("AnimationState") == 5)
                        {
                            animator.SetInteger("AnimationState", (int)5);
                        }
                        else
                        {
                            animator.SetInteger("AnimationState", (int)6);
                        }
                        
                    }
                }
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    gameManager.hasDrawn = true;
                }
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    gameManager.hasSwitchedW = true;
                }
            }
            else
            {
                /*if (Input.GetKey(KeyCode.W))
                {
                    if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
                    {
                        rotate(45);
                    }
                    else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
                    {
                        rotate(315);
                    }
                    else
                    {
                        rotate(0);
                    }
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    if (!Input.GetKey(KeyCode.S))
                    {
                        rotate(270);
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        rotate(225);
                    }
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
                    {
                        rotate(135);
                    }
                    else
                    {
                        rotate(180);
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    rotate(90);
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    GameObject[] enemies;
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    float distance = Mathf.Infinity;
                    int j = 0;
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        if (Vector2.Distance(new Vector2(enemies[i].transform.position.x, enemies[i].transform.position.y), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)) < distance
                                && InCone2D(gameObject.transform, new Vector2(enemies[i].transform.position.x, enemies[i].transform.position.y), Mathf.Infinity, 22f))
                        {
                            distance = Vector2.Distance(new Vector2(enemies[i].transform.position.x, enemies[i].transform.position.y), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
                            j = i;
                        }
                    }
                    float changeInX = enemies[j].transform.position.x - gameObject.transform.position.x;
                    float changeInY = enemies[j].transform.position.y - gameObject.transform.position.y;
                    if (distance != Mathf.Infinity)
                    {
                        //gameObject.transform.position = ne
                        //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Mathf.Atan(difference.x / difference.y) * 180 / Mathf.PI + 90);
                        //gameObject.transform.LookAt(new Vector3(enemies[j].transform.position.x, enemies[j].transform.position.y, enemies[j].transform.position.z), -transform.forward);
                        //LookAtDirection(gameObject, enemies[j].transform.position, Vector3.up);
                        float rotation = -Mathf.Abs(Mathf.Atan(changeInX / changeInY) * Mathf.Rad2Deg);
                        gameObject.transform.eulerAngles = new Vector3(0, 0, rotation);
                    }*/
                float changeInX = GetCurrentWorldPoint().x - gameObject.transform.position.x;
                float changeInY = GetCurrentWorldPoint().y - gameObject.transform.position.y;
                if (changeInY >= 0)
                {
                    float rotation = -(Mathf.Atan(changeInX / changeInY) * Mathf.Rad2Deg);
                    gameObject.transform.eulerAngles = new Vector3(0, 0, rotation);
                }
                else
                {
                    gameManager.hasAimed = true;
                    float rotation = -(Mathf.Atan(changeInX / changeInY) * Mathf.Rad2Deg) - 180;
                    gameObject.transform.eulerAngles = new Vector3(0, 0, rotation);
                }
                
            }
            }

        }

        /*if (Input.GetKey(KeyCode.LeftAlt) && Time.time >= nextDash)
        {
            nextDash = Time.time + cooldown;
            Dash();
        }*/
    
    /*private void Dash()
    {
        transform.position += transform.up * dashSpeed;
    }*/
    private void rotateAndMove(float angle)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
        transform.position += transform.up * Time.deltaTime * speed;
    }
    private void rotate (float angle)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
    }
    

    // adds wait time to shot based on speed of bow
    IEnumerator shotCooldown()
    {
        yield return new WaitForSeconds(bowStats.speed);
        if (animator.GetInteger("AnimationState") == 2 || animator.GetInteger("AnimationState") == 4 || animator.GetInteger("AnimationState") == 6)
        {
            animator.SetInteger("AnimationState", (int)6);
        }
        else
        {
            animator.SetInteger("AnimationState", (int)5);
        }
        canShoot = true;
    }

    public static bool InCone2D(Transform player, Vector2 target, float MaxDist, float MaxAngle) { 
        return Vector2.Distance(target, player.position) < MaxDist && Vector2.Angle(player.forward, (target - new Vector2(player.position.x, player.position.y)).normalized) < MaxAngle; 
    }
    public static Quaternion LookAtDirection(GameObject looker, Vector3 lookAtDirection, Vector3 lookerForwardVector)
    {
        Transform inputTransform = looker.transform;
        inputTransform.rotation = Quaternion.LookRotation(lookAtDirection); //calculating look rotation

        //Changes the LookAt alignment, like you can use "Vector3.up" vector instead of default "Vector3.forward"
        inputTransform.Rotate(GetForwardVectorAngleOffest(lookerForwardVector), Space.Self);

        return inputTransform.rotation;
    }

    static Vector3 GetForwardVectorAngleOffest(Vector3 forwardVector)
    {
        if (forwardVector == Vector3.up)
            return new Vector3(90, 0, 0);

        if (forwardVector == Vector3.right)
            return new Vector3(0, -90, 0);

        return new Vector3(0, 0, 0); ;
    }
    private Vector2 GetCurrentWorldPoint()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private IEnumerator displayOutOfAmmoText() // enables out of ammo text for a short period of time
    {
        outOfAmmoTextActive = true;
        outOfAmmoText.text = "Out of Ammo";
        yield return new WaitForSeconds(1);
        outOfAmmoTextActive = false;
        outOfAmmoText.text = "";
    } 
}
