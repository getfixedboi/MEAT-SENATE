using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [HideInInspector] public float Damage;
    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyBehaviour>())
        {
            other.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
