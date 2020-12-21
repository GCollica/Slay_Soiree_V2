using System.Collections.Generic;
using System;
using UnityEngine;

public class SkeletonEnemy_Attacking : MonoBehaviour
{
    private SkeletonEnemy_AI aiComponent;
    private SkeletonEnemy_Pathfinding pathfindingComponent;

    public GameObject attackParent;
    
    private Transform attackPos;

    public List<GameObject> attackTargets;

    public RaycastHit2D[] raycastHits;

    public bool inAttackRange = false;
    public bool isAttacking = false;
    public bool attackCoolingDown = false;

    //private float attackSequenceTimer = 0f;
    //private float attackSequenceDuration = 1f;

    public float attackCoolDownTimer = 0f;
    public float attackCoolDownDuration = 1.5f;

    private void Awake()
    {
        aiComponent = this.gameObject.GetComponentInParent<SkeletonEnemy_AI>();
        pathfindingComponent = this.gameObject.GetComponentInParent<SkeletonEnemy_Pathfinding>();
        attackPos = attackParent.transform.GetChild(0);
    }

    //Sets characters attack direction.
    public void SetAttackDirection()
    {
        attackParent.transform.right = -(aiComponent.currentTargetTransform.transform.position - attackParent.transform.position).normalized;

    }

    //Executes attacks based on targets chosen from the AttackRaycast function. *Needs check as it throws an error*
    public void ExecuteAttacks()
    {
        if (attackTargets.Count <= 0)
        {
            return;
        }
        
        if (attackTargets.Count > 0)
        {
            foreach (GameObject target in attackTargets)
            {
                target.GetComponent<PlayerStats>().TakeDamage(aiComponent.basicEnemy1Script.basicEnemyClass.currentDamage);

                if(aiComponent.quirkManager.CurrentQuirk.quirkID == 2)
                {
                    aiComponent.quirkManager.SpawnGoldPouch(target);
                }
            }
        }
    }

    public void RunAttackCooldownTimer()
    {
        if (attackCoolingDown == true)
        {
            if (attackCoolDownTimer < attackCoolDownDuration)
            {
                attackCoolDownTimer += Time.deltaTime;
            }
            if (attackCoolDownTimer >= attackCoolDownDuration)
            {
                attackCoolingDown = false;
            }
        }
    }

    //Turns on and off circlecasts used for attack sequence, attackPos input is used for the sequential nature of the check.
    public void AttackRaycast()
    {
        raycastHits = Physics2D.CircleCastAll(attackPos.position, 0.25f, (this.transform.position - attackPos.position));

        if (raycastHits.Length > 0)
        {
            foreach (RaycastHit2D hit in raycastHits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //Debug.Log("Player " + hit.collider.gameObject.name.ToString() + " Collider Hit");

                    if (attackTargets.Contains(hit.collider.gameObject) != true)
                    {
                        attackTargets.Add(hit.collider.gameObject);
                    }
                }
            }

            Array.Clear(raycastHits, 0, raycastHits.Length);
        }

    }

    public void CombinedAttackFunction()
    {
        pathfindingComponent.LockMovement();
        SetAttackDirection();
        AttackRaycast();
        aiComponent.currentAIState = SkeletonEnemy_AI.AIState.ExecutingAttacks;
    }

    public void ToggleAttackCollider(bool input)
    {
        Collider2D collider = this.gameObject.GetComponent<CircleCollider2D>();
        collider.enabled = input;
    }

    //Basic check for whether the enemy is within attack range of the current target.
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (collider.gameObject == aiComponent.currentTargetTransform.gameObject)
            {
                inAttackRange = true;
            }
        }
    }

    //Basic check for whether the enemy has left attack range of the current target.
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (collider.gameObject == aiComponent.currentTargetTransform.gameObject)
            {
                inAttackRange = false;
            }
        }
    }

    //Small function to find a child Gameobject given the parent and childs name.
    private GameObject FindChildGameObject(GameObject parent, string childName)
    {
        GameObject result;

        result = parent.transform.Find(childName).gameObject;

        return result;
    }

}
