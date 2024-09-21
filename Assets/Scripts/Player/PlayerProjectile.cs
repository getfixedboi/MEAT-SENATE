using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [HideInInspector] public float Damage;
    private void Start()
    {
        if (PlayerStatictics.CurrentModifier)
        {
            PlayerStatictics.CurrentModifier.ModRef.AttachProjectileEffect(gameObject);
        }
        Destroy(gameObject, 3f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyBehaviour>())
        {
            other.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(Damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Static"))
        {
            Destroy(this.gameObject);
        }
    }
}
