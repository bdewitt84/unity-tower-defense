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
    [SerializeField] private float reward = 5;
    [SerializeField] private float damage = 5;
    private Material enemyMaterial;
    [SerializeField] Color originalColor;


    private Renderer[] enemyRenderers;
    private List<Material> enemyMaterials = new();
    private List<Color> originalColors = new();

    [SerializeField] private Color hitFlashColor = Color.red; // Color when hit
    [SerializeField] private float hitFlashDuration = 0.1f; // Total time for flashing


    private void OnEnable()
    {
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnGameClear += HandleStageClear;
    }


    private void OnDisable()
    {
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnGameClear -= HandleStageClear;
    }


    // Start is called once before the first execution of Update after the
    // MonoBehaviour is created
    private void Start()
    {
        InitializeMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        // Health
        if (OutOfHealth())
        {
            BroadcastEnemyKilledEvent();
            Die();
        }

        // Pathing

        pathing.Update();
        if (pathing.HasReachedGoal())
        {
            BroadcastEnemyReachedGoalEvent();
            Die();
        }

        // Movement
        Vector3 destination = pathing.GetCurrentWaypointPosition();
        MoveTowards(destination);
        if (AtPosition(destination))
        {
            AlignWithPosition(destination);
        }
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
        enemyRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in enemyRenderers)
        {
            // Clone each material so changes don't affect shared material
            Material clonedMat = new Material(renderer.material);
            clonedMat.color = originalColor;
            renderer.material = clonedMat;
            enemyMaterials.Add(clonedMat);
            originalColors.Add(clonedMat.color);
        }
    }


    //
    // Private Helper functions
    //
    public bool AtCurrentWaypoint()
    {
        float margin = 0.1f;
        float distanceFromWaypoint = Vector3.Distance(transform.position,
                                                        pathing.GetCurrentWaypointPosition());
        return (distanceFromWaypoint < margin);
    }

    public bool AtPosition(Vector3 position)
    {
        float margin = 0.1f;
        float distanceFromWaypoint = Vector3.Distance(transform.position,
                                                        position);
        return (distanceFromWaypoint < margin);
    }

    private void AlignWithPosition(Vector3 position)
    {
        transform.position = position;
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
        for (int i = 0; i < enemyMaterials.Count; i++)
        {
            enemyMaterials[i].color = hitFlashColor;
        }

        yield return new WaitForSeconds(hitFlashDuration);

        for (int i = 0; i < enemyMaterials.Count; i++)
        {
            enemyMaterials[i].color = originalColors[i];
        }
    }


    private void BroadcastEnemyReachedGoalEvent()
    {
        GameEvents.EnemyReachedGoal(this, damage);
    }

    private void BroadcastEnemyKilledEvent()
    {
        GameEvents.EnemyKilled(this, reward);
    }

    private bool OutOfHealth()
    {
        return health <= 0;
    }

    public void SetPathfindingComponent(PathfindingComponent component)
    {
        pathing = component;
    }

    private void HandleStageClear()
    {
        Die();
    }

    private void HandleGameOver()
    {
        Die();
    }
}



