using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerStatictics : MonoBehaviour
{
    #region movment
    [Header("Movment")]
    public float BaseMovSpeed;
    public float JumpForce;
    #endregion
    #region params
    [Header("Parameters")]
    public float MaxHP;
    public float СurrentHP;
    private bool _invincibility = false;
    [SerializeField][Range(0.1f, 40f)] private float _invincibilityPeriod;
    private static HashSet<ItemBehaviour> _playerItems;
    #endregion
    [Header("UI")]
    [SerializeField] private Canvas _playerCanvas;
    [SerializeField] private GameObject _itemImagePrefab;
    [SerializeField] private UnityEngine.UI.Text _healthText;
    private static List<GameObject> _uiPrefabs = new List<GameObject>();
    public Vector3 Offset;

    private void Awake()
    {
        _playerItems = new HashSet<ItemBehaviour>();
        СurrentHP = MaxHP;

        _healthText.text = MaxHP.ToString() + "/" + СurrentHP.ToString();
    }
    private void Update()
    {
        _healthText.text = MaxHP.ToString() + "/" + СurrentHP.ToString();
    }

    public void Heal(float healAmount)
    {
        СurrentHP += healAmount;
        if (СurrentHP > MaxHP)
        {
            СurrentHP = MaxHP;
        }
    }

    public void TakeDamage(float inflictedDamage)
    {
        if (_invincibility) { return; }

        СurrentHP -= inflictedDamage;

        StartCoroutine(C_TemporaryInvinsibility());
    }
    private IEnumerator C_TemporaryInvinsibility()
    {
        _invincibility = true;
        yield return new WaitForSeconds(_invincibilityPeriod);
        _invincibility = false;
    }

    public void AddItem(ItemBehaviour item)
    {
        if (!_playerItems.Contains(item))
        {
            TakeFromGroundItem(item.gameObject);

            _playerItems.Add(item);
            ReloadUI();
        }
        else
        {
            throw new ArgumentException($"Item {item.name} is already taken");
        }
    }
    public void RemoveItem(ItemBehaviour item)
    {
        if (_playerItems.Contains(item))
        {
            _playerItems.Remove(item);
            ReloadUI();

            ThrowAwayItem(item.gameObject);
        }
        else
        {
            throw new ArgumentException($"Item {item.name} does not exists");
        }
    }
    private void ReloadUI()
    {
        // Удаляем старые UI элементы
        for (int i = 0; i < _uiPrefabs.Count; i++)
        {
            Destroy(_uiPrefabs[i]);
        }
        _uiPrefabs.Clear();

        int horizontalDelay = 0;
        foreach (ItemBehaviour item in _playerItems)
        {
            Vector3 extraOffset = new Vector3(horizontalDelay * 150, 0, 0);
            horizontalDelay++;

            // Создаем новый экземпляр префаба
            GameObject gameObj = Instantiate(_itemImagePrefab, _playerCanvas.transform.position + Offset + extraOffset, Quaternion.identity);

            // Добавляем компонент только к экземпляру
            ItemBehaviour itemRef = item.GetComponent<ItemBehaviour>();
            gameObj.GetComponent<ShowItemDescOnUI>().ItemRef = itemRef;

            // Настраиваем компонент ShowItemDescOnUI
            var showItemDesc = gameObj.GetComponent<ShowItemDescOnUI>();
            if (showItemDesc != null)
            {
                showItemDesc.Canvas = _playerCanvas;
            }

            // Устанавливаем родителя для позиционирования UI элемента
            gameObj.transform.SetParent(_playerCanvas.transform, false);

            // Добавляем созданный экземпляр в список для дальнейшего использования
            _uiPrefabs.Add(gameObj);
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
        Vector3 targetPosition = transform.position + transform.forward * 0.5f - transform.up * .4f;

        while (Vector3.Distance(item.transform.position, targetPosition) > 0.1f)
        {
            // Динамическое движение предмета к цели с заданной скоростью
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Ожидаем до следующего кадра
        }

        // Убедимся, что предмет точно находится в целевой позиции
        item.transform.position = targetPosition;

        // Делает item дочерним объектом игрока
        item.transform.SetParent(transform);

        // Перемещаем item к позиции игрока (можно добавить смещение при необходимости)
        item.transform.localPosition = Vector3.zero;

        // Отключаем item, чтобы он не был видимым или интерактивным в мире
        item.SetActive(false);
    }

    private void ThrowAwayItem(GameObject item)
    {
        // Включаем item, чтобы он снова был видимым и интерактивным
        item.SetActive(true);

        // Удаляем item из дочерних объектов игрока
        item.transform.SetParent(null);

        // Устанавливаем позицию выбрасывания перед игроком
        item.transform.position = transform.position + transform.forward;

        // Включаем физику для предмета
        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            // Добавляем силу для "выброса" предмета вперед
            rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
        }

        // Запускаем корутину для включения коллайдера с задержкой
        StartCoroutine(EnableColliderAfterDelay(item, 0.1f)); // Задержка в 0.1 секунды
    }

    private IEnumerator EnableColliderAfterDelay(GameObject item, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Включаем коллайдер предмета после задержки
        if (item.TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = true;
        }
    }

}
