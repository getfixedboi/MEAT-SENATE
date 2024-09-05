using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeatPiece : MonoBehaviour
{
    private static GameObject _player;
    private Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player");
    }
    private void Start()
    {
        StartCoroutine(C_EnablePhysCollider());
        StartCoroutine(C_DisablePhysCollider());
    }
    private IEnumerator C_EnablePhysCollider()
    {
        yield return new WaitForSeconds(.15f);
        GetComponent<BoxCollider>().enabled = true;
    }
    private IEnumerator C_DisablePhysCollider()
    {
        yield return new WaitForSeconds(.5f);
        if (_rb.velocity == Vector3.zero)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TakeFromGroundItem(gameObject);
        }
    }
    private void TakeFromGroundItem(GameObject item)
    {
        // Отключаем физику у предмета, если она есть, чтобы она не мешала движению
        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        // Отключаем коллайдер предмета
        if (item.TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }

        // Запускаем корутину для динамического перемещения предмета к игроку
        StartCoroutine(AnimateItemAbsorption(item));
    }

    private IEnumerator AnimateItemAbsorption(GameObject item)
    {
        float speed = 20f; // Скорость перемещения предмета к игроку

        // Целевая позиция для предмета (например, на уровне "живота", чуть ниже камеры)
        Vector3 targetPosition = _player.transform.position + _player.transform.forward * 0.5f - _player.transform.up * .4f;

        while (Vector3.Distance(item.transform.position, targetPosition) > 0.1f)
        {
            // Динамическое движение предмета к цели с заданной скоростью
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Ожидаем до следующего кадра
        }
        // Убедимся, что предмет точно находится в целевой позиции
        item.transform.position = targetPosition;

        // Перемещаем item к позиции игрока (можно добавить смещение при необходимости)
        item.transform.localPosition = Vector3.zero;

        _player.GetComponent<PlayerSkills>().MeatPieceCount++;
        Destroy(gameObject);
    }
}
