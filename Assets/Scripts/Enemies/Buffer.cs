using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Buffer : EnemyBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        SetTarget();
    }
    protected override void Update()
    {
        try
        {
            base.Update();
        }
        catch (Exception ex)
        {
            if (ex is MissingReferenceException || ex is NullReferenceException)
            {
                SetTarget();
            }
        }
        if (target)
        {
            agent.SetDestination(target.position);
        }
    }

    private void SetTarget()
    {
        List<EnemyOne> enemies = GameObject.FindObjectsOfType<EnemyOne>()?.ToList();
        if (enemies.Count > 0)
        {
            float min = enemies.Min(e => e.DistanceToPlayer);
            target = enemies.First(e => e.DistanceToPlayer == min).gameObject.transform;
        }

        if (!target)
        {
            try
            {
                target = GameObject.FindObjectsOfType<EnemyBehaviour>().Where(e => e is not Buffer).ToList().FirstOrDefault().gameObject.transform;
            }
            catch (NullReferenceException)
            {
                target = this.transform;
            }
        }
    }
}
