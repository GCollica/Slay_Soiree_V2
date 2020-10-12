using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyAIAttacking : MonoBehaviour
{
    private Enemy_AI aiComponent;

    private GameObject attackParent;
    
    //private Transform attackPos1;
    private Transform attackPos2;
    //private Transform attackPos3;

    public List<GameObject> attackTargets;


    public RaycastHit2D[] raycastHits;

    public bool inAttackRange = false;
    public bool isAttacking = false;
    public bool attackCoolingDown = false;

    //private float attackSequenceTimer = 0f;
    //private float attackSequenceDuration = 1f;

    public float attackCoolDownTimer = 0f;
    public float attackCoolDownDuration = 3f;

    private void Awake()
    {
        aiComponent = this.gameObject.GetComponent<Enemy_AI>();
        attackParent = FindChildGameObject(this.gameObject, "Attack_Direction");
        //attackPos1 = FindChildGameObject(attackParent, "Attack_Pos1").transform;
        attackPos2 = FindChildGameObject(attackParent, "Attack_Pos2").transform;
        //attackPos3 = FindChildGameObject(attackParent, "Attack_Pos3").transform;
    }

    //Sets characters attack direction.
    public void SetAttackDirection()
    {
        attackParent.transform.right = (aiComponent.currentTargetTransform.transform.position - attackParent.transform.position);

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
                Debug.Log("Executed attack on " + target.name);
                target.GetComponent<PlayerStats>().TakeDamage(aiComponent.basicEnemy1Script.basicEnemyClass.currentDamage);
                //Perform Attacks.
                //attackTargets.Remove(target);
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
    public void AttackRaycast(int attackPos)
    {
        switch (attackPos)
        {
            /*case 1:

                raycastHits = Physics2D.CircleCastAll(attackPos1.position, 1f, (this.transform.position - attackPos1.position));

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
                break;*/

            case 2:

                raycastHits = Physics2D.CircleCastAll(attackPos2.position, 1f, (this.transform.position - attackPos2.position));

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
                break;

            /*case 3:

                raycastHits = Physics2D.CircleCastAll(attackPos3.position, 1f, (this.transform.position - attackPos3.position));

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
                break;*/

            default:
                break;

        }
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
