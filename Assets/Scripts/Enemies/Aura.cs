using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float radius = GetComponent<SphereCollider>().radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemy))
            {
                enemy.InAura = false;
            }
        }
    }

}
