using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PlayerCombat playerCombat;
    public GameObject bulletMaster;

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Arrow Hit");
            collision.GetComponent<SkeletonEnemy>().TakeDamage(bulletMaster, "Light");
            Destroy(gameObject);
        }
    }
}
