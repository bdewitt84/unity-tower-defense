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

// Author: Minsu Kim 
// 

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform lane;
    [SerializeField] private List<Transform> waypoints = new();
    [SerializeField] private int waypointIndex = 0;
    private Vector3 destination;
    private Transform currentWaypoint;

    [SerializeField] private float speed = 4f;
    [SerializeField] private float health = 100;

    private Material enemyMaterial;
    [SerializeField] private Color hitFlashColor = Color.red; // Color when hit
    [SerializeField] private float hitFlashDuration = 0.5f; // Total time for flashing


    // Start is called once before the first execution of Update after the
    // MonoBehaviour is created
    private void Start()
    {
        if (lane == null)
        {
            Debug.LogError("[EnemyController] Lane not assigned. Destroying self.");
            Die();
            return;
        }

        GetWaypointsFromLane();

        if (waypoints.Count == 0)
        {
            Debug.LogError("[EnemyController] No waypoints found in lane. Destroying self.");
            Die();
            return;
        }

        SetNextWaypoint();
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
        if (AtCurrentWaypoint())
        {
            AlignWithWaypoint();
            if (VisitedAllWaypoints())
            {
                // animation?
                // play sounds?
                BroadcastEnemyReachedGoalEvent();
                Die();
            }
            else
            {
                SetNextWaypoint();
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

    // Sets the current waypoint to the next in the waypoints list, and
    // unpacks its x and z coordinates into a destination vector for the
    // enemy to move toward. The y coordinate is kept the same to ensure
    // that the enemy's height does not change.
    private void SetNextWaypoint()
    {
        if (waypoints == null)
        {
            Debug.Log("Cannot set next waypoint. Waypoints is null.");
            Die();
            return;
        }

        if (waypoints.Count == 0)
        {
            Debug.Log("Cannot set next waypoint. Waypoints list is empty.");
            Die();
            return;
        }

        if (waypointIndex < 0 || waypointIndex >= waypoints.Count)
        {
            Debug.Log("Cannt set next waypoint. Index is out of range.");
            Die();
            return;
        }

        currentWaypoint = waypoints[waypointIndex];
        destination = new Vector3(currentWaypoint.transform.position.x,
                                  transform.position.y,
                                  currentWaypoint.transform.position.z);
        waypointIndex += 1;

        // Error handling could include
        //  trying to set the next waypoint
        //  trying to set the final waypoint,
        //  calling HandleReachedGoal()
        //  calling HandleLostAllHealth()
        //  or just Die()
    }

    // Returns true if the enemy's distance to the current waypoint is within
    // a specified margin
    private bool AtCurrentWaypoint()
    {
        if (currentWaypoint == null)
        {
            Debug.LogWarning("[EnemyController] Current waypoint is null.");
            return false;
        }
        else
        {
            float margin = 0.1f;
            float distanceFromWaypoint = Vector3.Distance(transform.position,
                                                          destination);
            return (distanceFromWaypoint < margin);
        }
    }

    // Returns true if the last visited waypoint was the final waypoint in the
    // waypoints list
    private bool VisitedAllWaypoints()
    {
        return (waypointIndex >= lane.childCount);
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

}



