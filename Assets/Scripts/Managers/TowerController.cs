using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// Author: Brett DeWitt, Minsu Kim
/// 
/// Created: 04.27.2025
/// 
/// Description:
///
/// Basic Tower object that detects and attacks enemies within range.
/// Handles targeting, firing, and cooldown


public class TowerController : MonoBehaviour
{
    private EnemyController currentTarget;
    [SerializeField] private float maxRange = 6f;
    [SerializeField] private float damage = 5f;
    private float currentCooldown = 0f;
    [SerializeField] private float maxCooldown = 2f;

    [SerializeField] private Color fireFlashColor = Color.blue;
    [SerializeField] private float fireFlashDuration = 0.1f;
    [SerializeField] private int cost = 10;

    private bool isFlashing = false;

    private Renderer[] towerRenderers;
    private List<Material> towerMaterials = new();
    private List<Color> originalColors = new();

    void Start()
    {
        InitializeMaterials();
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= HandleGameOver;
    }

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

    public int GetCost()
    {
        return cost;
    }

    private bool HasTarget() => currentTarget != null;

    private bool WeaponIsReady() => currentCooldown <= 0;

    private bool TargetIsInRange()
    {
        if (currentTarget == null) return false;
        float targetDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
        return targetDistance < maxRange;
    }

    private void FireAtTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.TakeDamage(damage);
            FindObjectOfType<AudioManager>()?.PlayTowerFireSound(); // For Sound Effect
            if (!isFlashing)
            {
                StartCoroutine(FlashTower());
            }
            currentCooldown = maxCooldown;
        }
    }

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

    private void CoolDownWeapon()
    {
        currentCooldown -= Time.deltaTime;
    }

    private void InitializeMaterials()
    {
        towerRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in towerRenderers)
        {
            Material clonedMat = new Material(renderer.material);
            renderer.material = clonedMat;
            towerMaterials.Add(clonedMat);
            originalColors.Add(clonedMat.color);
        }
    }

    private IEnumerator FlashTower()
    {
        isFlashing = true;

        for (int i = 0; i < towerMaterials.Count; i++)
        {
            towerMaterials[i].color = fireFlashColor;
        }

        yield return new WaitForSeconds(fireFlashDuration);

        for (int i = 0; i < towerMaterials.Count; i++)
        {
            towerMaterials[i].color = originalColors[i];
        }

        isFlashing = false;
    }

    private void HandleGameOver()
    {
        Destroy(gameObject);
    }
}
