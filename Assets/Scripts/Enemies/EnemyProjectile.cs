using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [HideInInspector] public float Damage;
    private void Start()
    {
        Destroy(gameObject, 8f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerStatictics>().TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
