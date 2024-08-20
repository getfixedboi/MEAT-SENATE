using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : EnemyBehaviour
{
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
            if (timer >= 1.5f)
            {
                agent.speed = movSpeedList[1];
            }
            else
            {
                agent.speed = movSpeedList[0];
            }
        }
        else if (distanceToPlayer <= rangeList[1])
        {
            agent.speed = 0;
            agent.SetDestination(gameObject.transform.position);

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
        Debug.Log($"deal to player some damage: {damageList[0]}");
        target.gameObject.GetComponent<PlayerStatictics>().TakeDamage(damageList[0]);
    }
    private void HeavyAttack()
    {
        totalCooldownTimer = cooldownList[1];
        timer = 0;
        Debug.Log($"deal to player some damage: {damageList[1]}");
        target.gameObject.GetComponent<PlayerStatictics>().TakeDamage(damageList[1]);
    }
}
