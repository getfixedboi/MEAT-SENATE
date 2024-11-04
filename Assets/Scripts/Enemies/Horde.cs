using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde : MonoBehaviour
{
    [SerializeField] private List<EnemyBehaviour> _enemies;
    private void Start()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].GetComponent<EnemyBehaviour>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].GetComponent<EnemyBehaviour>().enabled = true;
            }
            Destroy(gameObject);
        }
    }
}
