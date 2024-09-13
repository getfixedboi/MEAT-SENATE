using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerStatictics : MonoBehaviour
{
    #region movment
    [Header("Movement")]
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
    private static HashSet<ModifierBehaviour> _playerModifiers;
    #endregion
    [Header("UI")]
    [SerializeField] private Canvas _playerCanvas;
    [SerializeField] private GameObject _itemImagePrefab;
    [SerializeField] private GameObject _modifierImagePrefab;
    [SerializeField] private UnityEngine.UI.Text _healthText;
    private static List<GameObject> _itemUiPrefabs = new List<GameObject>();
    private static List<GameObject> _modifierUiPrefabs = new List<GameObject>();
    public static ShowModifierDescOnUI CurrentModifier;
    public Vector3 ItemOffset;
    public Vector3 ModifierOffset;

    private void Awake()
    {
        _playerItems = new HashSet<ItemBehaviour>();
        _playerModifiers = new HashSet<ModifierBehaviour>();
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
#region items management
    public void AddItem(ItemBehaviour item)
    {
        if (!_playerItems.Contains(item))
        {
            TakeFromGroundItem(item.gameObject);

            _playerItems.Add(item);
            ReloadItemUI();
        }
        else
        {
            throw new ArgumentException($"Item {item.name} is already taken");
        }
    }
    public void AddItem(ItemBehaviour item, bool param)
    {
        if (!_playerItems.Contains(item))
        {
            if (param)
            {
                item.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            TakeFromGroundItem(item.gameObject, param);

            _playerItems.Add(item);
            ReloadItemUI();
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
            ReloadItemUI();

            ThrowAwayItem(item.gameObject);
        }
        else
        {
            throw new ArgumentException($"Item {item.name} does not exists");
        }
    }
    private void ReloadItemUI()
    {
        // Удаляем старые UI элементы
        for (int i = 0; i < _itemUiPrefabs.Count; i++)
        {
            Destroy(_itemUiPrefabs[i]);
        }
        _itemUiPrefabs.Clear();

        int horizontalDelay = 0;
        foreach (ItemBehaviour item in _playerItems)
        {
            Vector3 extraOffset = new Vector3(horizontalDelay * 150, 0, 0);
            horizontalDelay++;

            // Создаем новый экземпляр префаба
            GameObject gameObj = Instantiate(_itemImagePrefab, _playerCanvas.transform.position + ItemOffset + extraOffset, Quaternion.identity);

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
            _itemUiPrefabs.Add(gameObj);
        }
    }
#endregion
#region modifiers management
public void AddModifier(ModifierBehaviour item)
    {
        if (!_playerModifiers.Contains(item))
        {
            TakeFromGroundItem(item.gameObject);

            _playerModifiers.Add(item);
            ReloadModifierUI();
        }
        else
        {
            throw new ArgumentException($"Modifier {item.name} is already taken");
        }
    }
    public void AddModifier(ModifierBehaviour item, bool param)
    {
        if (!_playerModifiers.Contains(item))
        {
            if (param)
            {
                item.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            TakeFromGroundItem(item.gameObject, param);

            _playerModifiers.Add(item);
            ReloadModifierUI();
        }
        else
        {
            throw new ArgumentException($"Modifier {item.name} is already taken");
        }
    }
    public void RemoveModifier(ModifierBehaviour item)
    {
        if (_playerModifiers.Contains(item))
        {
            _playerModifiers.Remove(item);
            ReloadModifierUI();

            ThrowAwayItem(item.gameObject);
        }
        else
        {
            throw new ArgumentException($"Item {item.name} does not exists");
        }
    }
    private void ReloadModifierUI()
    {
        // Удаляем старые UI элементы
        for (int i = 0; i < _modifierUiPrefabs.Count; i++)
        {
            Destroy(_modifierUiPrefabs[i]);
        }
        _modifierUiPrefabs.Clear();

        int horizontalDelay = 0;
        foreach (ModifierBehaviour mod in _playerModifiers)
        {
            Vector3 extraOffset = new Vector3(horizontalDelay * 150, 0, 0);
            horizontalDelay--;

            // Создаем новый экземпляр префаба
            GameObject gameObj = Instantiate(_modifierImagePrefab, _playerCanvas.transform.position + ModifierOffset + extraOffset, Quaternion.identity);

            // Добавляем компонент только к экземпляру
            ModifierBehaviour modRef = mod.GetComponent<ModifierBehaviour>();
            gameObj.GetComponent<ShowModifierDescOnUI>().ModRef = modRef;

            // Настраиваем компонент ShowModDescOnUI
            var showModDesc = gameObj.GetComponent<ShowModifierDescOnUI>();
            if (showModDesc != null)
            {
                showModDesc.Canvas = _playerCanvas;
            }

            // Устанавливаем родителя для позиционирования UI элемента
            gameObj.transform.SetParent(_playerCanvas.transform, false);

            // Добавляем созданный экземпляр в список для дальнейшего использования
            _modifierUiPrefabs.Add(gameObj);
        }
    }
#endregion
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
    private void TakeFromGroundItem(GameObject item, bool param)
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
        StartCoroutine(AnimateItemAbsorption(item, param));
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
    private IEnumerator AnimateItemAbsorption(GameObject item, bool param)
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

        if (param)
        {
            item.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

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
