using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWall : MonoBehaviour
{
    private int health;
    private int maxHealth;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>(); // adds an edgeCollider that checks for collisions
        edgeCollider.isTrigger = true;
        gameObject.tag = "Wall"; // sets the tag to "wall". Helps enemies identify that they've hit a wall and should bounce back as a result
        lineRenderer = GetComponent<LineRenderer>();
        maxHealth = lineRenderer.positionCount; // saves the max health for opacity purposes
        health = lineRenderer.positionCount; // gets the number of vertices in the line and uses that as the health
        //lineRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        //Debug.Log("script started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        health--;
        //lineRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, ((float) health / (float) maxHealth));
        Debug.Log((float) health / (float) maxHealth);
        if (health <= 0) { // destroy self if health hits 0
            Destroy(gameObject);
        }
    }
}
