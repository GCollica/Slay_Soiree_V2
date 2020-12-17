using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PlayerCombat playerCombat;
    public GameObject bulletMaster;
    private SoundManager soundManager;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Arrow Hit");
            soundManager.Play("Arrow Hit");
            var impactSkeleton = collision.GetComponent<SkeletonEnemy>();
            var impactBird = collision.GetComponent<CrowEnemy>();

            if(impactSkeleton != null)
            {
                impactSkeleton.TakeDamage(bulletMaster, "Light");
                Destroy(gameObject);
            }
            else if(impactBird != null)
            {
                impactBird.TakeDamage(bulletMaster, "Light");
            }

            Destroy(gameObject);
        }
        else if(collision.tag == "Boss")
        {
            Debug.Log("Arrow Hit");
            soundManager.Play("Arrow Hit");
            collision.transform.parent.transform.parent.transform.GetComponentInParent<BossEnemy>().TakeDamage(bulletMaster, "Light");
            Destroy(gameObject);
        }
    }
}
