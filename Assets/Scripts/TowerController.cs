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

    [SerializeField] private int cost = 10;

    private Vector3 _firingPosition = new Vector3(0.0f, 2.5f, 0.0f);

    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _projectileSpeed;


    void Start()
    {

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
            GameEvents.TowerFired();
            Projectile projectile = Instantiate(_projectilePrefab);
            projectile.Initialize(currentTarget, _projectileSpeed, damage);
            projectile.transform.position = transform.position + _firingPosition;
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

    private void HandleGameOver()
    {
        Destroy(gameObject);
    }
}
