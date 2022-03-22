using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveArrow : MonoBehaviour
{
    public int damage;
    public float speed;
    public GameObject explosion;
    // Start is called before the first frame update
    void Awake()
    {
        transform.position += transform.up * 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 10 || transform.position.x < -10 || transform.position.y > 6 || transform.position.y < -6)
        {
            Destroy(gameObject);
        }
        transform.position += transform.up * Time.deltaTime * speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hit = collision.collider.gameObject;
        if (!hit.CompareTag("Player"))
        {
            if (hit.CompareTag("Enemy"))
            {
                gameObject.GetComponent<Enemy>().DealDamage(damage);
                Explode();
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        if (!hit.CompareTag("Player"))
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Enemy>().DealDamage(damage);
                Explode();
            }
            Destroy(gameObject);
        }
    }
    private void Explode()
    {
        Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
    }
}
