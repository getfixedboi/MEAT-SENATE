using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeatPiece : MonoBehaviour
{
    private static GameObject _player;
    private Rigidbody _rb;
    private bool _canBeStopped = false;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player");
    }
    private void Start()
    {
        StartCoroutine(C_DisableCollider());
    }
    public void Update()
    {

        if (_rb.velocity != Vector3.zero)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        if (_rb.velocity == Vector3.zero && _canBeStopped)
        {
            transform.Rotate(0, 0.1f, 0);
            _rb.isKinematic = true;
            _rb.useGravity = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    private IEnumerator C_DisableCollider()
    {
        yield return new WaitForSeconds(.5f);
        _canBeStopped = true;
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
        float speed = 3f; // Скорость перемещения предмета к игроку

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
