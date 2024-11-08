using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

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
        Profiler.BeginSample("SetBufferTarget");

        List<EnemyBehaviour> enemies = new List<EnemyBehaviour>(GameObject.FindObjectsOfType<EnemyBehaviour>());

        if (enemies.Count > 0)
        {
            EnemyOne closestEnemy = null;
            float minDistance = float.MaxValue;

            foreach (var enemy in enemies)
            {
                if (enemy is not EnemyOne) { continue; }

                if (enemy.DistanceToPlayer < minDistance)
                {
                    minDistance = enemy.DistanceToPlayer;
                    closestEnemy = enemy as EnemyOne;
                }
            }

            if (closestEnemy != null)
            {
                target = closestEnemy.transform;
                return;
            }
        }

        foreach (var enemy in enemies)
        {
            if (enemy is not Buffer)
            {
                target = enemy.transform;
                return;
            }
        }

        target = this.transform;

        Profiler.EndSample();
    }
}
