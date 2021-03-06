﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowEnemy_AI : MonoBehaviour
{
    //Data references for state machine components of the code.
    public enum AIState { Idle, FindingTarget, PursuingTarget, AttachedToTarget };
    public AIState currentAIState = AIState.Idle;

    //Data references for multi-use components of the code.
    public List<GameObject> totalNonCrowEnemies;
    public GameObject currentTarget;
    public Transform currentTargetTransform;
    public Transform flypointLeft;
    public Transform flypointRight;

    //Date references for idle state of the code.
    public float idleDelayTimer = 0f;
    private float idleDelayLength = 1f;

    private float facingDirectionBuffer = 0.125f;

    private float attachDistance = 0.5f;

    private CrowEnemyAIPathfinding pathfindingComponent;
    private SkeletonEnemy_Animation animationComponent;
    public CrowEnemy crowEnemyScript;
    public QuirkManager quirkManager;

    void Awake()
    {
        pathfindingComponent = this.gameObject.GetComponent<CrowEnemyAIPathfinding>();
        crowEnemyScript = this.gameObject.GetComponent<CrowEnemy>();
        animationComponent = this.gameObject.transform.GetComponentInChildren<SkeletonEnemy_Animation>();
        quirkManager = FindObjectOfType<QuirkManager>();
    }

    void Update()
    {
        switch (currentAIState)
        {
            case AIState.Idle:

                pathfindingComponent.ClearPath();
                currentTarget = null;
                currentTargetTransform = null;

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
                break;

            case AIState.PursuingTarget:
                if (currentTarget == null || currentTargetTransform == null)
                {
                    currentAIState = AIState.FindingTarget;
                }

                SetFacingDirection();
                pathfindingComponent.PursureTarget();

                if (Vector2.Distance(this.transform.position, currentTargetTransform.position) <= attachDistance)
                {
                    currentAIState = AIState.AttachedToTarget;
                }
                break;

            case AIState.AttachedToTarget:
                if (currentTarget == null || currentTargetTransform == null)
                {
                    currentAIState = AIState.FindingTarget;
                }

                flypointLeft.position = new Vector3(currentTargetTransform.position.x - 0.5f, currentTargetTransform.position.y, currentTargetTransform.position.z);
                flypointRight.position = new Vector3(currentTargetTransform.position.x + 0.5f, currentTargetTransform.position.y, currentTargetTransform.position.z);
                SetFacingDirection();
                pathfindingComponent.PursueFlyPoint();
                break;

            default:
                break;
        }

    }

    //Initialises totalPlayers array.
    private void InitialiseTargets()
    {

        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        totalNonCrowEnemies.Clear();

        foreach (var enemy in totalEnemies)
        {
            if(enemy.GetComponent<CrowEnemy>() == null)
            {
                totalNonCrowEnemies.Add(enemy);
            }

            if(totalNonCrowEnemies.Count <= 0)
            {
                currentAIState = AIState.Idle;
            }

            else
            {
                FindNearestTarget();
            }
        }
    }

    //Finds current closest target to this enemy.
    private void FindNearestTarget()
    {

        float closestDistance = (totalNonCrowEnemies[0].transform.position - this.gameObject.transform.position).magnitude;
        GameObject closestEnemy = totalNonCrowEnemies[0];

        foreach (GameObject enemy in totalNonCrowEnemies)
        {
            if ((enemy.transform.position - this.gameObject.transform.position).magnitude < closestDistance)
            {
                closestDistance = (enemy.transform.position - this.gameObject.transform.position).magnitude;
                closestEnemy = enemy;
            }
        }

        currentTarget = closestEnemy;
        currentTargetTransform = closestEnemy.transform;
        closestEnemy.GetComponent<SkeletonEnemy>().AddAttachedCrow(this.gameObject);

        currentAIState = AIState.PursuingTarget;
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
