using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowEnemy_Animation : MonoBehaviour
{
    private Animator animator;
    private CrowEnemy baseCrowClass;
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        animator = this.GetComponent<Animator>();

        if (this.gameObject.transform.GetComponentInParent<CrowEnemy>() != null)
        {
            baseCrowClass = this.gameObject.transform.GetComponentInParent<CrowEnemy>();
        }

    }

    public void SetAnimBool(string boolNameInput, bool targetValue)
    {
        if (animator == null)
        {
            Debug.Log("No animator set");
            return;
        }
        if (animator.GetBool(boolNameInput) == targetValue)
        {
            //Debug.Log("Animation bool already set to target value");
            return;
        }

        animator.SetBool(boolNameInput, targetValue);
        //Debug.Log("Changed animation bool");
    }

    public void KillEnemy()
    {
        if(baseCrowClass == null)
        {
            return;
        }

        baseCrowClass.EnemyDead();

    }

    public void DeathAudio()
    {
        //soundManager.Play("Skeleton Death");
    }
}
