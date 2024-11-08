using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Aura : MonoBehaviour
{
    [SerializeField][Range(0f, 100f)] private float _damageBoost;
    private void FixedUpdate()
    {
        transform.Rotate(0.1f, 1, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemy);
        if (enemy)
        {
            enemy.InAura = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemy);
        if (enemy)
        {
            enemy.InAura = false;
        }
    }
    private void OnDestroy()
    {
        Profiler.BeginSample("AuraDestroy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, 7.5f);
        //фиксированное значение радиуса снижает время кадра на 24ms ахуеть юнити
        //спасибо за этот ренат логан коричневого цвета 20 века

        //GetComponent<SphereCollider>().radius и 
        //Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z) харам!
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemy))
            {
                enemy.InAura = false;
            }
        }

        Profiler.EndSample();
    }
}
