using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCanvas : MonoBehaviour
{
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        Vector3 targetPosition = _player.transform.position;
        targetPosition.y = transform.position.y; // Сбросить высоту цели до уровня текущего объекта

        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // Повернуть только по оси Y
    }
}
