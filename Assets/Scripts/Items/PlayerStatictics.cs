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
    [NonSerialized] private float _currentHP;
    public float MaxArmor;
    [NonSerialized] private float _currentArmor;
    [SerializeField][Min(0.1f)] private float _armorStartTimeRegen;
    [SerializeField][Min(0.1f)] private float _armorTickPeriodRegen;
    [NonSerialized] private bool _invincibility = false;
    [SerializeField][Range(0.1f, 40f)] private float _invincibilityPeriod;
    [NonSerialized] private static HashSet<ItemBehaviour> _playerItems;
    #endregion
    [Header("UI")]
    [SerializeField] private Canvas _playerCanvas;
    [SerializeField] private GameObject _itemImagePrefab;
    private static List<GameObject> _uiPrefabs = new List<GameObject>();
    public Vector3 Offset;

    private void Awake()
    {
        _playerItems = new HashSet<ItemBehaviour>();
        _currentHP = MaxHP;
        _currentArmor = MaxArmor;
    }

    public void TakeDamage(float inflictedDamage)
    {
        if (_invincibility) { return; }

        float residualDamage = _currentArmor - inflictedDamage;
        if (residualDamage >= 0)
        {
            _currentArmor -= inflictedDamage;
        }
        else
        {
            _currentArmor -= inflictedDamage - UnityEngine.Mathf.Abs(residualDamage);
            _currentHP -= UnityEngine.Mathf.Abs(residualDamage);
        }
        StopCoroutine(C_ArmorRegeneration());
        StartCoroutine(C_ArmorRegeneration());

        StartCoroutine(C_TemporaryInvinsibility());
    }
    private IEnumerator C_ArmorRegeneration()
    {
        yield return new WaitForSeconds(_armorStartTimeRegen);

        while (_currentArmor < MaxArmor)
        {
            _currentArmor += 1;
            yield return new WaitForSeconds(_armorTickPeriodRegen);
        }
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
            Type itemType = item.GetType();
            gameObj.AddComponent(itemType);

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


}
