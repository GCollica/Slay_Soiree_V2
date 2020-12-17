using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float movementSpeed = 200f;
    private Rigidbody2D rBody;
    private Transform targetDestination;

    private float deathTimer = 0f;
    private float deathTimeThreshold = 1f;

    private void Awake()
    {
        rBody = this.gameObject.GetComponent<Rigidbody2D>();
        targetDestination = this.gameObject.transform.parent.gameObject.transform.parent.GetChild(0);
    }
    // Update is called once per frame
    void Update()
    {
        if(deathTimer < deathTimeThreshold)
        {
            deathTimer += Time.deltaTime;
            MoveBullet();
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void MoveBullet()
    {
        Vector2 directionVector = (targetDestination.position - this.gameObject.transform.position).normalized;
        Vector2 force = directionVector * movementSpeed * Time.deltaTime;
        rBody.AddForce(force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(collision.gameObject.name + " got stuck by the glock");
            collision.GetComponent<PlayerStats>().TakeDamage(1000f);
            Destroy(this.gameObject);
        }
    }
}
