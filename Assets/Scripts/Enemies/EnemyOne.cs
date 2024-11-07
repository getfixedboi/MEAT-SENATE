using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : EnemyBehaviour
{
    protected override void Update()
    {
        base.Update();
        if (DistanceToPlayer >= rangeList[0])
        {
            agent.SetDestination(gameObject.transform.position);
        }
        else if (DistanceToPlayer <= rangeList[0] && DistanceToPlayer >= rangeList[1])
        {
            agent.updateRotation = true;
            agent.SetDestination(target.position);
            if (timer >= 1.5f)
            {
                agent.speed = movSpeedList[1];
            }
            else
            {
                agent.speed = movSpeedList[0];
            }
        }
        else if (DistanceToPlayer <= rangeList[1])
        {
            agent.updateRotation = false;
            agent.speed = 0;
            agent.SetDestination(gameObject.transform.position);

            if (totalCooldownTimer >= 0 && totalCooldownTimer < 0.5)
            {
                SelfRotateTowardsTarget();
            }

            if (totalCooldownTimer >= 0) { return; }

            float random = UnityEngine.Random.Range(0, 2);

            if (random == 0)
            {
                HeavyAttack();
            }
            else
            {
                CommomAttack();
            }
        }
    }
    private void CommomAttack()
    {
        totalCooldownTimer = cooldownList[0];
        timer = 0;
        target.gameObject.GetComponent<PlayerStatictics>().TakeDamage(damageList[0] * (InAura ? 2 : 1));
    }
    private void HeavyAttack()
    {
        totalCooldownTimer = cooldownList[1];
        timer = 0;
        target.gameObject.GetComponent<PlayerStatictics>().TakeDamage(damageList[1] * (InAura ? 2 : 1));
    }
    private void SelfRotateTowardsTarget()
    {
        agent.updateRotation = false;
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
    }
}
