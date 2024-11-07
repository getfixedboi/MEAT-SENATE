using UnityEngine;
using UnityEngine.AI;

public class EnemyTwo : EnemyBehaviour
{
    [SerializeField] private GameObject _projectile;
    #region dodge
    private float sideMovementTimer = 0f;  // Таймер для управления движением влево-вправо
    private float sideMovementDuration = 1f;  // Продолжительность движения в одном направлении
    private float pauseTimer = 0f;  // Таймер для перерыва между движениями
    private float pauseDuration = 0.5f;  // Длительность перерыва между движениями
    private float randomValue;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        agent.stoppingDistance = rangeList[1];
    }

    protected override void Update()
    {
        base.Update();

        if (DistanceToPlayer >= rangeList[0])
        {
            agent.speed = movSpeedList[0];
            agent.SetDestination(transform.position);
            agent.updateRotation = true;
        }
        else if (DistanceToPlayer <= rangeList[0] && DistanceToPlayer >= rangeList[1])
        {
            agent.speed = movSpeedList[0];
            agent.SetDestination(target.position);
            SelfRotateTowardsTarget();
        }
        else
        {
            SideToSideMovement();

            if (totalCooldownTimer >= 0 && totalCooldownTimer < 0.5)
            {
                SelfRotateTowardsTarget();
            }

            if (totalCooldownTimer >= 0) { return; }

            Shoot();
        }
    }

    private void Shoot()
    {
        totalCooldownTimer = cooldownList[0];

        GameObject bullet = Instantiate(_projectile, transform.position, Quaternion.identity);
        bullet.GetComponent<EnemyProjectile>().Damage = damageList[0] * (InAura ? 2 : 1);

        Vector3 direction = (target.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = direction * attackSpeedList[0];
    }

    private void SelfRotateTowardsTarget()
    {
        agent.updateRotation = false;
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
    }

    private void SideToSideMovement()
    {
        if (pauseTimer > 0)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        sideMovementTimer += Time.deltaTime;

        if (sideMovementTimer >= sideMovementDuration)
        {
            randomValue = Random.value;
            sideMovementTimer = 0f;
            pauseTimer = pauseDuration;  // перерыв
        }

        Vector3 sideDirection;
        if (randomValue < 0.2)//вправо
        {
            sideDirection = transform.right;
        }
        else if (randomValue < .4 && randomValue > .2)//влево
        {
            sideDirection = -transform.right;
        }
        else if (randomValue < .6 && randomValue > .4)//назад
        {
            sideDirection = -transform.forward;
        }
        else if (randomValue < .8 && randomValue > .6)//вправо назад
        {
            sideDirection = -transform.forward + transform.right;
        }
        else //влево назад
        {
            sideDirection = -transform.forward - transform.right;
        }
        transform.position += sideDirection * movSpeedList[0] * Time.deltaTime;
    }
}
