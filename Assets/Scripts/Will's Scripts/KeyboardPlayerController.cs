using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardPlayerController : MonoBehaviour
{
    public float dashSpeed;
    public float speed;
    public GameObject arrow;
    //private float nextDash = 0;
    public float cooldown;
    private GameManager gameManager;
    public GameObject arrow2;
    private int equippedArrow = 1;
    // damage muliplier
    public float damageX;
    // currrent bow the playe has
    public GameObject bow;
    private Bow bowStats;
    public bool canShoot = true;
    private GeneralArrow arrowStats;
    // Start is called before the first frame update
    void Start()
    {
        bowStats = bow.GetComponent<Bow>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // gets arrow and arrow stats
        GameObject usedArrow = arrow;
        if (equippedArrow == 1 || Input.GetKeyDown(KeyCode.Alpha1))
        {
            usedArrow = arrow;
            equippedArrow = 1;
            arrowStats = usedArrow.GetComponent<GeneralArrow>();
        }
        if (equippedArrow == 2 || Input.GetKeyDown(KeyCode.Alpha2))
        {
            usedArrow = arrow2;
            equippedArrow = 2;
            arrowStats = usedArrow.GetComponent<GeneralArrow>();
        }
        if(!gameManager.IsGameOver())
        {
            if (Input.GetKeyDown(KeyCode.Space) && canShoot && arrowStats.remaining > 0)
            {
                Debug.Log("shoot");
                Vector3 location = transform.position;
                Quaternion rotation = transform.rotation;
                Instantiate(usedArrow, location, rotation);
                canShoot = false;
                StartCoroutine(shotCooldown());
                if (!arrowStats.infinite)
                {
                    arrowStats.remaining -= 1;
                }
            }
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.W))
                {
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
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    if (!Input.GetKey(KeyCode.S))
                    {
                        rotateAndMove(270);
                    }    
                    if (Input.GetKey(KeyCode.S))
                    {
                        rotateAndMove(225);
                    }
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
                    {
                        rotateAndMove(135);
                    }
                    else
                    {
                        rotateAndMove(180);
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    rotateAndMove(90);
                } 
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
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
            }

        }

        /*if (Input.GetKey(KeyCode.LeftAlt) && Time.time >= nextDash)
        {
            nextDash = Time.time + cooldown;
            Dash();
        }*/
    }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Money"))
        {
            gameManager.UpdateMoney(1);
            Destroy(collision.gameObject);
        }
    }

    // adds wait time to shot based on speed of bow
    IEnumerator shotCooldown()
    {
        yield return new WaitForSeconds(bowStats.speed);

        canShoot = true;
    }
}
