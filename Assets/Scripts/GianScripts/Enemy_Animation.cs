using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Animation : MonoBehaviour
{
    private Animator animator;
    private BasicEnemy1 baseEnemyClass;
    private CrowEnemy baseCrowEnemyClass;
    private EnemyAIAttacking skeletonAttackComponent;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();

        if (this.gameObject.transform.GetComponentInParent<BasicEnemy1>() != null)
        {
            baseEnemyClass = this.gameObject.transform.GetComponentInParent<BasicEnemy1>();
            skeletonAttackComponent = this.gameObject.transform.parent.GetComponentInChildren<EnemyAIAttacking>();
        }
        if (this.gameObject.transform.GetComponentInParent<CrowEnemy>() != null)
        {
            baseCrowEnemyClass = this.gameObject.transform.GetComponentInParent<CrowEnemy>();
        }
        
    }

    public void SetAnimBool(string boolNameInput, bool targetValue)
    {
        if (!animator.GetBool(boolNameInput))
        {
            return;
        }
        if(animator.GetBool(boolNameInput) == targetValue)
        {
            return;
        }
        
        animator.SetBool(boolNameInput, targetValue);
    }

    public void KillEnemy()
    {
        if(baseEnemyClass != null)
        {
            baseEnemyClass.EnemyDead();
        }
        if(baseCrowEnemyClass != null)
        {
            baseCrowEnemyClass.EnemyDead();
        }

    }

    public void ExitFlinch()
    {
        animator.SetBool("Flinching", false);
    }

    public void ExitSkeleAttack()
    {
        animator.SetBool("Attacking", false);
    }

    public void ExecuteSkeleAttack()
    {
        skeletonAttackComponent.CombinedAttackFunction();
    }
}
