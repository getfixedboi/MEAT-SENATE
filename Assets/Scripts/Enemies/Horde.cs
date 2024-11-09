using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Horde : MonoBehaviour
{
    [SerializeField] private List<EnemyBehaviour> _meleeEnemies;
    [SerializeField] private List<EnemyBehaviour> _rangeEnemies;
    [SerializeField] private int _hordeCost = 3;
    private BoxCollider _spawnArea;
    private List<EnemyBehaviour> spawnedEnemies;

    private void Awake()
    {
        _spawnArea = GetComponent<BoxCollider>();
        spawnedEnemies = new List<EnemyBehaviour>();
    }

    private void Start()
    {
        SpawnHorde();
    }

    private void SpawnHorde()
    {
        List<EnemyBehaviour> allEnemies = new List<EnemyBehaviour>();
        allEnemies.AddRange(_meleeEnemies);
        allEnemies.AddRange(_rangeEnemies);

        while (_hordeCost > 0)
        {
            EnemyBehaviour enemyToSpawn = ChooseRandomEnemy(allEnemies);
            if (enemyToSpawn != null)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                EnemyBehaviour enemyInstance = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                spawnedEnemies.Add(enemyInstance);
                enemyInstance.enabled = false;
            }
            else
            {
                break;
            }
        }
    }

    private EnemyBehaviour ChooseRandomEnemy(List<EnemyBehaviour> allEnemies)
    {
        List<EnemyBehaviour> affordableEnemies = allEnemies.FindAll(e => e.SpawnCost <= _hordeCost);
        
        if (affordableEnemies.Count == 0)
        {
            return null;
        }

        int index = Random.Range(0, affordableEnemies.Count);
        EnemyBehaviour enemy = affordableEnemies[index];
        
        //Debug.Log(enemy.SpawnCost);

        _hordeCost -= enemy.SpawnCost;
        return enemy;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = _spawnArea.bounds.center;
        Vector3 extents = _spawnArea.bounds.extents;

        float x = Random.Range(center.x - extents.x, center.x + extents.x);
        float z = Random.Range(center.z - extents.z, center.z + extents.z);
        float y = center.y - extents.y; // Используем нижнюю точку BoxCollider'а для Y

        return new Vector3(x, y, z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (var enemy in spawnedEnemies)
            {
                enemy.enabled = true;
            }
            Destroy(this.gameObject);
        }
    }
}
