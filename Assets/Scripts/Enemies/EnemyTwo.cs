using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTwo : EnemyBehaviour
{
    [SerializeField] private GameObject _projectile;
    protected override void Awake()
    {
        maxHP = 2;
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
        if (distanceToPlayer >= rangeList[0])
        {
            agent.SetDestination(gameObject.transform.position);
        }
        else if (distanceToPlayer <= rangeList[0] && distanceToPlayer >= rangeList[1])
        {
            agent.SetDestination(target.position);
        }
        else if (distanceToPlayer <= rangeList[1])
        {
            agent.speed = 0;
            agent.SetDestination(gameObject.transform.position);

            if (totalCooldownTimer >= 0) { return; }

            Shoot();
        }
    }

    private void Shoot()
    {
        totalCooldownTimer = cooldownList[0];

        GameObject bullet = GameObject.Instantiate(_projectile, gameObject.transform.position, new Quaternion());
        bullet.GetComponent<EnemyProjectile>().Damage = damageList[0];

        Vector3 direction = (target.position - gameObject.transform.position).normalized;

        float bulletSpeed = attackSpeedList[0];
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
    }
}