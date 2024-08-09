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
        float random = UnityEngine.Random.Range(0,2);
        if (distanceToPlayer <= rangeList[0])
        {
            agent.speed = movSpeedList[0];
            agent.SetDestination(gameObject.transform.position);
            
            if(random==0)
            {
                HeavyAttack();
            }
            else
            {
                CommomAttack();
            }
            
        }
        else
        {
            agent.SetDestination(target.position);
            if (timer >= 1.5f)
            {
                agent.speed = movSpeedList[1];
            }
        }

        timer += Time.deltaTime;
    }
    private void CommomAttack()
    {
        if (timer >= cooldownList[0])
        {
            timer = 0;
            Debug.Log($"deal to player some damage: {damageList[0]}");
        }
    }
    private void HeavyAttack()
    {
        if (timer >= cooldownList[1])
        {
            timer = 0;
            Debug.Log($"deal to player some damage: {damageList[1]}");
        }
    }
}
