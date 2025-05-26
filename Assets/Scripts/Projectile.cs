using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private EnemyController _target;
    private float _speed;
    private float _damage;

    const float _distanceThreshold = 0.1f; // How close Projectile must be to _target to have 'arrived'

    public void Initialize(EnemyController target, float speed, float damage)
    {
        _target = target;
        _speed = speed;
        _damage = damage;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!TargetExists())
        {
            Die();
        }
        else if (ArrivedAtTarget())
        {
            DealDamageToTarget();
            Die();
        }
        else 
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);
    }

    private void DealDamageToTarget()
    {
        _target.TakeDamage(_damage);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private bool ArrivedAtTarget()
    {
        return _distanceThreshold > Vector3.Distance(this.transform.position, _target.transform.position);
    }

    private bool TargetExists()
    {
        return _target != null;
    }
}
