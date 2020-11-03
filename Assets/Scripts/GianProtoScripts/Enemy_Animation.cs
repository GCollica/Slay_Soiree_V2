using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Animation : MonoBehaviour
{
    private Animator animator;
    private BasicEnemy1 baseEnemyClass;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        baseEnemyClass = this.gameObject.transform.GetComponentInParent<BasicEnemy1>();
    }

    public void SetAnimBool(string boolNameInput, bool targetValue)
    {
        if(animator.GetBool(boolNameInput) == targetValue)
        {
            return;
        }
        
        animator.SetBool(boolNameInput, targetValue);
    }

    public void KillEnemy()
    {
        baseEnemyClass.EnemyDead();
    }
}
