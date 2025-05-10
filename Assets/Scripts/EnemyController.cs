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
// Must be assigned a PathfindingComponent using the constructor or by using
// the public method SetPathfindingComponent().
//
// Damage can be dealt to the enemy by using the pulblic function TakeDamage()
// Example:
//    EnemyController enemy = hitEnemyGameObject.GetComponent<EnemyController>();
//    enemy.TakeDamage(10);


public class EnemyController : MonoBehaviour
{
    [SerializeField] private PathfindingComponent pathing;

    [SerializeField] private float speed = 4f;
    [SerializeField] private float health = 100;

    private Material enemyMaterial;
    Color originalColor;

    [SerializeField] private Color hitFlashColor = Color.red; // Color when hit
    [SerializeField] private float hitFlashDuration = 0.5f; // Total time for flashing


    // Start is called once before the first execution of Update after the
    // MonoBehaviour is created
    private void Start()
    {
        InitializeMaterial();
        

        string reason;
        if (pathing.CanSetNextWaypoint(out reason))
        {
            pathing.SetNextWaypoint();
            // destination = pathing.GetCurrentWaypointPosition();
        }
        else
        {
            Die();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // Health/body
        if (OutOfHealth())
        {
            BroadcaseEnemyKilledEvent();
            Die();
        }

        // pathing
        if (pathing.AtCurrentWaypoint())
        {
            // movement?
            AlignWithWaypoint();
            // pathing
            if (pathing.ReachedGoal())
            {
                BroadcastEnemyReachedGoalEvent();
                Die();
            }
            else
            {
                // pathing
                string reason;
                if (pathing.CanSetNextWaypoint(out reason))
                {
                    // pathing
                    pathing.SetNextWaypoint();
                    // destination = pathing.GetCurrentWaypointPosition();
                }
                else
                {
                    Die();
                }
            }
        }
        // movement
        MoveTowards(pathing.GetCurrentWaypointPosition());
    }

    //
    // Public API functions
    //


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
        originalColor = enemyMaterial.color;
    }

    //
    // Private Helper functions
    //


    // Sets current position to the current waypoint, making sure enemy
    // accuratey follows the path
    private void AlignWithWaypoint()
    {
        transform.position = pathing.GetCurrentWaypointPosition();
        // Handle cases
        //   waypoint is null
    }


    private void MoveTowards(Vector3 destination)
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
        // Color originalColor = enemyMaterial.color;

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

}



