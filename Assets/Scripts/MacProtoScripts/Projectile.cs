using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PlayerCombat playerCombat;
    public GameObject bulletMaster;

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<BasicEnemy1>().TakeDamage(bulletMaster, "Light");
        }
    }
}
