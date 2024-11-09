using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [HideInInspector] public float Damage;

    private void Start()
    {
        Destroy(this.gameObject, 1f);
    }

    private void Update()
    {
        transform.Rotate(0, 2, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyBehaviour enemy = null;
        other.gameObject.TryGetComponent<EnemyBehaviour>(out enemy);
        if (enemy !=null)
        {
            enemy.TakeDamage(Damage);
            return;
        }

        PlayerStatictics player = null;
        other.gameObject.TryGetComponent<PlayerStatictics>(out player);
        if (player != null)
        {
            player.TakeDamage(Damage);
            return;
        }
    }
}
