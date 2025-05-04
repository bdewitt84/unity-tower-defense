using System;
using UnityEngine;
using System.Collections;


/// Author: Brett DeWitt
/// 
/// Created: 04.27.2025
/// 
/// Description:
///
/// Basic Tower object that detects and attacks enemies within range.
/// Handles targeting, firing, and cooldown


public class NewMonoBehaviourScript : MonoBehaviour
{
    private EnemyController currentTarget;
    [SerializeField] private float maxRange = 6f;
    [SerializeField] private float damage = 5f;
    private float currentCooldown = 0f;
    [SerializeField] private float maxCooldown = 2f;

    private Material towerMaterial; // The material of the tower
    [SerializeField] private Color fireFlashColor = Color.blue; // Color when firing
    [SerializeField] private float fireFlashDuration = 0.3f; // Flash duration

    private bool isFlashing = false; // To prevent multiple flashes running simultaneously

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the tower's material
        towerMaterial = GetComponent<Renderer>().material;

        // Check if the material is found
        if (towerMaterial == null)
        {
            Debug.LogError("Tower's material is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HasTarget() && TargetIsInRange())
        {
            if (WeaponIsReady())
            {
                FireAtTarget();
            }
            else
            {
                CoolDownWeapon();
            }
        }
        else
        {
            TryToFindNewTarget();
        }
    }

    // Checks if the tower currently has a target
    private bool HasTarget()
    {
        return (currentTarget != null);
    }

    // Checks if the weapon cooldown has expired
    private bool WeaponIsReady()
    {
        return (currentCooldown <= 0);
    }

    // Checks if the target is within the tower's attack range.
    private bool TargetIsInRange()
    {
        float targetDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
        return (targetDistance < maxRange);
    }

    // Attacks the target, applies damage, and triggers the flash effect
    private void FireAtTarget()
    {
        currentTarget.TakeDamage(damage);
        if (!isFlashing) // Prevent multiple flashes from happening simultaneously
        {
            StartCoroutine(FlashTower()); // Start the flash when firing
        }
        currentCooldown = maxCooldown;
    }

    // Searches for a new target within range if no target is found or current target is lost
    private void TryToFindNewTarget()
    {
        EnemyController[] enemies = GameObject.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        foreach (EnemyController enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= maxRange)
            {
                currentTarget = enemy;
                break;
            }
        }
    }

    // Reduces the cooldown timer over time
    private void CoolDownWeapon()
    {
        currentCooldown -= Time.deltaTime;
    }

    // Flashes when firing
    private IEnumerator FlashTower()
    {
        // Set the flag to prevent overlapping flashes
        isFlashing = true;
        Color originalColor = towerMaterial.color;
        towerMaterial.color = fireFlashColor;
        yield return new WaitForSeconds(fireFlashDuration);
        towerMaterial.color = originalColor;
        isFlashing = false;
    }
}
