using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections;


// Author: Brett DeWitt

// Created: 4.24.2025

// Description:

// Prototype for basic enemy creep.
// Must be assigned a Lane. The Lane must have child objects which comprise the
// waypoints of the lane. The Enemy will visit each of the waypoints in
// sequence until all waypoints have been visited, at which point the goal
// sequence will be triggered. If the Enemy loses all of its health, the
// death sequence will be triggered.

// To assign a lane to the Enemy, use the public function SetLane().
// Example:
//    GameObject enemyGO = Instantiate(enemyPrefab,
//                                     spawnPoint.position,
//                                     spawnPoint.rotation);
//    EnemyController enemy = enemyGO.GetComponent<EnemyController>();
//    enemy.SetLane(laneTransform);

// Damage can be dealt to the enemy by using the pulblic function TakeDamage()
// Example:
//    EnemyController enemy = hitEnemyGameObject.GetComponent<EnemyController>();
//    enemy.TakeDamage(10);


public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform lane;
    [SerializeField] private List<Transform> waypoints = new();
    private Vector3 destination;
    [SerializeField] private PathfindingComponent pathing;

    [SerializeField] private float speed = 4f;
    [SerializeField] private float health = 100;

    private Material enemyMaterial;
    [SerializeField] private Color hitFlashColor = Color.red; // Color when hit
    [SerializeField] private float hitFlashDuration = 0.5f; // Total time for flashing


    // Start is called once before the first execution of Update after the
    // MonoBehaviour is created
    private void Start()
    {
        LanePathFinder pathFindingComponent = new(this, lane);
        SetPathfindingComponent(pathFindingComponent);

        string reason;
        if (pathing.CanSetNextWaypoint(out reason))
        {
            pathing.SetNextWaypoint();
        }
        else
        {
            Die();
        }
        
        InitializeMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        if (OutOfHealth())
        {
            BroadcaseEnemyKilledEvent();
            Die();
        }
        if (pathing.AtCurrentWaypoint())
        {
            AlignWithWaypoint();
            if (pathing.ReachedGoal())
            {
                // animation?
                // play sounds?
                BroadcastEnemyReachedGoalEvent();
                Die();
            }
            else
            {
                string reason;
                if (pathing.CanSetNextWaypoint(out reason))
                {
                    pathing.SetNextWaypoint();
                }
                else
                {
                    Die();
                }
            }
        }
        MoveTowardWaypoint();
    }

    //
    // Public API functions
    //


    // Sets the lane for the enemy to follow. Children of lane will be treated
    // as waypoints.
    // If lane has no waypoints, enemy will be destroyed.
    // Throws ArgumentNullException if lane is Null.
    public void SetLane(Transform lane)
    {
        if (lane == null)
        {
            throw new ArgumentNullException("Lane must not be null");
        }
        this.lane = lane;
        GetWaypointsFromLane();

        if (waypoints.Count <= 0)
        {
            Debug.LogError("[EnemyController] Lane has no waypoints. Destroying self.");
            Die();
        }
    }

    // Reduces enemy's health by the specified damage amount. When health falls
    // below zero, triggers the handler for losing all health.
    public void TakeDamage(float damage)
    {
        if (damage < 0f)
        {
            Debug.LogWarning("[EnemyController] Negative damage received. Clamping to 0.");
            damage = 0f;
        }
        health -= damage;
        StartCoroutine(FlashEnemy());
    }


    // Returns the current remaining health of the enemy
    public float GetCurrentHealth()
    {
        return health;
    }

    private void InitializeMaterial()
    {
        enemyMaterial = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = enemyMaterial;
    }

    //
    // Private Helper functions
    //


    // Adds every child of lane to list of waypoints for enemy to follow.
    // The lane must first be set with SetLane()
    private void GetWaypointsFromLane()
    {
        if (lane == null)
        {
            Debug.Log("GetWaypointsFromLane must have non-null lane. Use" +
                "SetLane() to assign a lane to Enemy.");
        }
        else
        {
            waypoints.Clear();
            foreach (Transform waypoint in lane)
            {
                waypoints.Add(waypoint);
            }
        }
    }

    // Sets current position to the current waypoint, making sure enemy
    // accuratey follows the path
    private void AlignWithWaypoint()
    {
        transform.position = destination;
        // Handle cases
        //   waypoint is null
    }


    // Moves towards the current waypoint
    private void MoveTowardWaypoint()
    {
        Vector3 moveTowards = Vector3.MoveTowards(transform.position,
                                                  destination,
                                                  speed * Time.deltaTime);
        transform.position = moveTowards;
    }

    // Destroys the game object, removing it from the game
    private void Die()
    {
        Destroy(gameObject);
    }

    // Flashes when hit
    private IEnumerator FlashEnemy()
    {
        // Store the original color of the material
        Color originalColor = enemyMaterial.color;

        // Change to the flash color (red)
        enemyMaterial.color = hitFlashColor;

        // Wait for the specified duration
        yield return new WaitForSeconds(hitFlashDuration);

        // Revert back to the original color
        enemyMaterial.color = originalColor;
    }

    private void BroadcastEnemyReachedGoalEvent()
    {
        GameEvents.EnemyReachedGoal(this);
    }

    private void BroadcaseEnemyKilledEvent()
    {
        GameEvents.EnemyKilled(this);
    }

    private bool OutOfHealth()
    {
        return health <= 0;
    }


    public void SetPathfindingComponent(PathfindingComponent component)
    {
        pathing = component;
    }

    public Vector3 GetDestination()
    {
        return destination;
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
    }
}



