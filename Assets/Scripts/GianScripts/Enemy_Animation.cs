using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Animation : MonoBehaviour
{
    private Animator animator;   
    public BasicEnemy1 baseEnemyClass;
    public CrowEnemy baseCrowEnemyClass;
    private EnemyAIAttacking skeletonAttackComponent;
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
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
        if(animator == null)
        {
            Debug.Log("No animator set");
        }

        /*if (!animator.GetBool(boolNameInput))
        {
            Debug.Log("Couldn't find animation bool");
            return;
        }*/
        if(animator.GetBool(boolNameInput) == targetValue)
        {
            //Debug.Log("Animation bool already set to target value");
            return;
        }
        
        animator.SetBool(boolNameInput, targetValue);
        //Debug.Log("Changed animation bool");
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

    public void DeathAudio()
    {
        soundManager.Play("Skeleton Death");
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
