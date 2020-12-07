using System.Collections;
using System;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    //Data references for state machine components of the code.
    public enum AIState {Idle, FindingTarget, PursuingTarget, AttackPrep, AttackSequence, ExecutingAttacks};
    public AIState currentAIState = AIState.Idle;

    //Data references for multi-use components of the code.
    public GameObject[] totalPlayers;
    public Transform currentTargetTransform;

    //Date references for idle state of the code.
    private float idleDelayTimer = 0f;
    private float idleDelayLength = 1f;

    private float attackPrepTimer = 0f;
    private float attackPrepLength = 1.5f;

    private float facingDirectionBuffer = 0.125f;

    //Make private later @gian.
    public float knockbackForce = 5000;

    private EnemyAIAttacking attackComponent;
    private EnemyAIPathfinding pathfindingComponent;
    private Enemy_Animation animationComponent;
    public BasicEnemy1 basicEnemy1Script;
    public QuirkManager quirkManager;

    void Awake()
    {
        attackComponent = this.gameObject.GetComponentInChildren<EnemyAIAttacking>();
        pathfindingComponent = this.gameObject.GetComponent<EnemyAIPathfinding>();
        basicEnemy1Script = this.gameObject.GetComponent<BasicEnemy1>();
        animationComponent = this.gameObject.transform.GetComponentInChildren<Enemy_Animation>();
        quirkManager = FindObjectOfType<QuirkManager>();
    }
  
    void Update()
    {
        switch (currentAIState)
        {
            case AIState.Idle:
                
                pathfindingComponent.ClearPath();
                attackComponent.RunAttackCooldownTimer();
                animationComponent.SetAnimBool("Walking", false);
                attackComponent.ToggleAttackCollider(false);

                if(idleDelayTimer < idleDelayLength)
                {
                    idleDelayTimer += Time.deltaTime;
                }
                else if(idleDelayTimer >= idleDelayLength)
                {
                    currentAIState = AIState.FindingTarget;
                }
                break;

            case AIState.FindingTarget:

                idleDelayTimer = 0f;
                attackPrepTimer = 0f;
                InitialiseTargets();
                FindNearestTarget();
                attackComponent.ToggleAttackCollider(true);
                animationComponent.SetAnimBool("Walking", false);
                attackComponent.RunAttackCooldownTimer();
                currentAIState = AIState.PursuingTarget;
                break;

            case AIState.PursuingTarget:

                SetFacingDirection();
                pathfindingComponent.PursureTarget();
                animationComponent.SetAnimBool("Walking", true);
                attackComponent.RunAttackCooldownTimer();
                if(attackComponent.inAttackRange == true)
                {
                    currentAIState = AIState.AttackPrep;
                }
                break;

            case AIState.AttackPrep:
                animationComponent.SetAnimBool("Walking", false);
                if (attackPrepTimer < attackPrepLength)
                {
                    attackPrepTimer += Time.deltaTime;
                }
                else
                {
                    currentAIState = AIState.AttackSequence;
                }
                break;
                

            case AIState.AttackSequence:

                attackComponent.isAttacking = true;
                attackComponent.RunAttackCooldownTimer();

                //This would be the case that the target player has moved out of this objects attack range, after stepping into it.
                if (attackComponent.inAttackRange == false)
                {
                    currentAIState = AIState.FindingTarget;
                }

                //This would be the case that the target player is within attack range & this objects attack isn't on cooldown.
                if(attackComponent.inAttackRange == true && attackComponent.attackCoolingDown == false)
                {
                    attackComponent.SetAttackDirection();
                    attackComponent.AttackRaycast(2);
                    currentAIState = AIState.ExecutingAttacks;
                }
                break;

            case AIState.ExecutingAttacks:
                
                if(attackComponent.isAttacking == true)
                {
                    attackComponent.ExecuteAttacks();
                    attackComponent.attackTargets.Clear();
                    //attackSequenceTimer = 0f;
                    attackComponent.attackCoolDownTimer = 0f;
                    attackComponent.attackCoolingDown = true;
                    attackComponent.isAttacking = false;
                }

                if (attackComponent.inAttackRange == false)
                {
                    currentAIState = AIState.FindingTarget;
                }
                else
                {
                    currentAIState = AIState.AttackSequence;
                }
                break;

            default:
                break;
        }

    }

    //Initialises totalPlayers array.
    private void InitialiseTargets()
    {
        totalPlayers = GameObject.FindGameObjectsWithTag("Player");
    }

    //Finds current closest target to this enemy.
    private void FindNearestTarget()
    {
        if(totalPlayers.Length == 0)
        {
            return;
        }
        
        float closestDistance = (totalPlayers[0].transform.position - this.gameObject.transform.position).magnitude;
        GameObject closestPlayer = totalPlayers[0];

        foreach (GameObject player in totalPlayers)
        {
            if((player.transform.position - this.gameObject.transform.position).magnitude < closestDistance)
            {
                closestPlayer = player;
            }
        }

        currentTargetTransform = closestPlayer.transform;
    }

    //Sets characters Facing Direction by flipping the sprite.
    private void SetFacingDirection()
    {
        if (pathfindingComponent.rigidBody.velocity.x > (0 + facingDirectionBuffer))
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (pathfindingComponent.rigidBody.velocity.x <= (0 - facingDirectionBuffer))
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
    }

    public void Knockback()
    {
        Debug.Log("Knockback");

        Vector2 directionVector = (currentTargetTransform.position - this.gameObject.transform.position).normalized;
        Vector2 force = -directionVector * knockbackForce;
        pathfindingComponent.rigidBody.AddForce(force);
    }
}