using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowEnemy_AI : MonoBehaviour
{
    //Data references for state machine components of the code.
    public enum AIState { Idle, FindingTarget, PursuingTarget, AttachedToTarget };
    public AIState currentAIState = AIState.Idle;

    //Data references for multi-use components of the code.
    public GameObject[] totalEnemies;
    public Transform currentTargetTransform;

    //Date references for idle state of the code.
    private float idleDelayTimer = 0f;
    private float idleDelayLength = 1f;

    private float facingDirectionBuffer = 0.125f;

    private float attachDistance = 0.5f;

    private EnemyAIPathfinding pathfindingComponent;
    private Enemy_Animation animationComponent;
    public CrowEnemy crowEnemyScript;
    public QuirkManager quirkManager;

    void Awake()
    {
        pathfindingComponent = this.gameObject.GetComponent<EnemyAIPathfinding>();
        crowEnemyScript = this.gameObject.GetComponent<CrowEnemy>();
        animationComponent = this.gameObject.transform.GetComponentInChildren<Enemy_Animation>();
        quirkManager = FindObjectOfType<QuirkManager>();
    }

    void Update()
    {
        switch (currentAIState)
        {
            case AIState.Idle:

                pathfindingComponent.ClearPath();
                animationComponent.SetAnimBool("Walking", false);

                if (idleDelayTimer < idleDelayLength)
                {
                    idleDelayTimer += Time.deltaTime;
                }
                else if (idleDelayTimer >= idleDelayLength)
                {
                    currentAIState = AIState.FindingTarget;
                }
                break;

            case AIState.FindingTarget:

                idleDelayTimer = 0f;
                InitialiseTargets();
                FindNearestTarget();
                animationComponent.SetAnimBool("Walking", false);
                currentAIState = AIState.PursuingTarget;
                break;

            case AIState.PursuingTarget:

                SetFacingDirection();
                pathfindingComponent.PursureTarget();
                animationComponent.SetAnimBool("Walking", true);
                if (Vector2.Distance(this.transform.position, currentTargetTransform.position) <= attachDistance)
                {
                    currentAIState = AIState.AttachedToTarget;
                }
                break;

            case AIState.AttachedToTarget:

                break;

            default:
                break;
        }

    }

    //Initialises totalPlayers array.
    private void InitialiseTargets()
    {
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    //Finds current closest target to this enemy.
    private void FindNearestTarget()
    {
        if (totalEnemies.Length == 0)
        {
            return;
        }

        float closestDistance = (totalEnemies[0].transform.position - this.gameObject.transform.position).magnitude;
        GameObject closestPlayer = totalEnemies[0];

        foreach (GameObject player in totalEnemies)
        {
            if ((player.transform.position - this.gameObject.transform.position).magnitude < closestDistance)
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
}
