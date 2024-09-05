using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidFuckingEnemyAnigillator : MonoBehaviour
{
    public float rayDistance = 10f;  // Distance for the raycast
    public float damage = 10f;       // Damage to apply

    void Update()
    {
        // Check if the right mouse button is clicked
        if (Input.GetMouseButtonDown(1))  // Right mouse button is button 1
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // Check if the object hit has a component inheriting from EnemyBehaviour
                EnemyBehaviour enemy = hit.collider.GetComponent<EnemyBehaviour>();

                if (enemy != null)
                {
                    // Call TakeDamage on the enemy
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
