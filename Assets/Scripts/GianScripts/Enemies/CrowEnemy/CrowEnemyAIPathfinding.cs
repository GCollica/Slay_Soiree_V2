﻿using System.Collections;
using System;
using UnityEngine;
using Pathfinding;

public class CrowEnemyAIPathfinding : MonoBehaviour
{
    public enum FlyPointState { left, right };
    public FlyPointState currentFlyPointState = FlyPointState.left;
    private CrowEnemy_AI aiComponent;
    //Data references for pathfinding components of the code.
    private Seeker seeker;
    private Path currentPath;

    public Rigidbody2D rigidBody;
    private bool creatingPath = false;
    private bool reachedEndOfPath = false;

    private int currentWaypoint = 0;

    //public float movementSpeed = 10f;
    private float nextWaypointDistance = 0.1f;
    private float updatePathTimer = 0f;
    private float updatePathInterval = .1f;

    private void Awake()
    {
        aiComponent = this.gameObject.GetComponent<CrowEnemy_AI>();
        seeker = this.gameObject.GetComponent<Seeker>();
        rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        //graphUpdater = FindObjectOfType<GraphUpdateObject>();
    }

    //Generates path towards the target postition
    public void GeneratePath()
    {
        //AstarPath.active.UpdateGraphs(GameObject.FindObjectOfType<GraphUpdateScene>().);
        creatingPath = true;
        seeker.StartPath(rigidBody.position, aiComponent.currentTargetTransform.position, OnPathComplete);
        updatePathTimer = 0f;
    }

    public void GeneratePathAttached()
    {
        creatingPath = true;

        if (currentFlyPointState == FlyPointState.left)
        {
            seeker.StartPath(rigidBody.position, aiComponent.flypointLeft.position, OnPathComplete);
        }
        if (currentFlyPointState == FlyPointState.right)
        {
            seeker.StartPath(rigidBody.position, aiComponent.flypointRight.position, OnPathComplete);
        }

        updatePathTimer = 0f;
    }

    //Checks to see if the path was created without an error, callback function for GeneratePath
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
            creatingPath = false;

            //Debug.Log("Created Path Successfully");
        }
        else
        {
            seeker.StartPath(rigidBody.position, aiComponent.currentTargetTransform.position, OnPathComplete);
        }
    }

    //Clears current path.
    public void ClearPath()
    {
        if (currentPath != null)
        {
            currentPath = null;
        }
    }

    //Function to run in update which creates a path for the ai to the target on an interval and moves the enemy along the path
    public void PursureTarget()
    {
        //Always want to generate path if this object has finished it's current path and not currently generating one.
        if (currentPath == null && creatingPath == false || reachedEndOfPath == true)
        {
            GeneratePath();
        }

        if (currentPath == null && creatingPath == true)
            return;

        //Advances the timer which is responsible for not creating a new path every frame.
        if (currentPath != null && creatingPath == false)
        {

            if (updatePathTimer < updatePathInterval)
            {
                updatePathTimer += Time.deltaTime;
            }

            if (updatePathTimer >= updatePathInterval)
            {
                GeneratePath();
            }

        }

        //Checks to see if this objects current waypoint (of it's current path) is the last waypoint of the path, updates the relevant bool
        if (currentWaypoint >= currentPath.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            if (reachedEndOfPath != false)
            {
                reachedEndOfPath = false;
            }
        }

        //Sets a direction vector pointing from this object to it's currentWaypoint.
        Vector2 directionVector = ((Vector2)currentPath.vectorPath[currentWaypoint] - rigidBody.position).normalized;
        Vector2 force = directionVector * aiComponent.crowEnemyScript.basicEnemyClass.currentMovementSpeed * Time.deltaTime;

        rigidBody.AddForce(force);

        float distance = Vector2.Distance(rigidBody.position, currentPath.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    public void PursueFlyPoint()
    {
        //Always want to generate path if this object has finished it's current path and not currently generating one.
        if (currentPath == null && creatingPath == false || reachedEndOfPath == true)
        {
            GeneratePathAttached();
        }

        if (currentPath == null && creatingPath == true)
            return;

        //Advances the timer which is responsible for not creating a new path every frame.
        if (currentPath != null && creatingPath == false)
        {

            if (updatePathTimer < updatePathInterval)
            {
                updatePathTimer += Time.deltaTime;
            }

            if (updatePathTimer >= updatePathInterval)
            {
                GeneratePathAttached();
            }

        }

        //Checks to see if this objects current waypoint (of it's current path) is the last waypoint of the path, updates the relevant bool
        if (currentWaypoint >= currentPath.vectorPath.Count)
        {
            reachedEndOfPath = true;
            if (currentFlyPointState == FlyPointState.left)
            {
                currentFlyPointState = FlyPointState.right;
                return;
            }
            else if (currentFlyPointState == FlyPointState.right)
            {
                currentFlyPointState = FlyPointState.left;
                return;
            }
        }
        else
        {
            if (reachedEndOfPath != false)
            {
                reachedEndOfPath = false;
            }
        }

        //Sets a direction vector pointing from this object to it's currentWaypoint.
        Vector2 directionVector = ((Vector2)currentPath.vectorPath[currentWaypoint] - rigidBody.position).normalized;
        Vector2 force = directionVector * aiComponent.crowEnemyScript.basicEnemyClass.currentMovementSpeed * Time.deltaTime;

        rigidBody.AddForce(force);

        float distance = Vector2.Distance(rigidBody.position, currentPath.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

}

