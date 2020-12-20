using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy_Animation : MonoBehaviour
{
    private Animator animator;   
    private SkeletonEnemy baseSkeletonClass;
    private SkeletonEnemy_Attacking attackComponent;
    private SkeletonEnemy_Pathfinding pathfindingComponent;
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        animator = this.GetComponent<Animator>();

        if(this.gameObject.transform.GetComponentInParent<SkeletonEnemy>() == null)
        {
            return;
        }

        baseSkeletonClass = this.gameObject.transform.GetComponentInParent<SkeletonEnemy>();
        attackComponent = this.gameObject.transform.parent.GetComponentInChildren<SkeletonEnemy_Attacking>();
        pathfindingComponent = this.gameObject.transform.GetComponentInParent<SkeletonEnemy_Pathfinding>();
        
    }

    public void SetAnimBool(string boolNameInput, bool targetValue)
    {
        if(animator == null)
        {
            Debug.Log("No animator set");
            return;
        }
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
        if(baseSkeletonClass == null)
        {
            return;
        }

        baseSkeletonClass.EnemyDead();

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
        if (animator.GetBool("Dying") != true)
        {
            pathfindingComponent.UnlockMovement();
        }
        animator.SetBool("Attacking", false);
    }

    public void ExecuteSkeleAttack()
    {
        attackComponent.CombinedAttackFunction();
    }
}
