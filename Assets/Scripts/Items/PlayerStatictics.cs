using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerStatictics : MonoBehaviour
{
    #region movment
    [Header("Movment")]
    [SerializeField][Range(0f, 10f)] private float BaseMovSpeed;
    [SerializeField][Range(0f, 100f)] private float JumpForce;
    #endregion
    [SerializeField][Range(0f, 100f)] private float _maxHP;
    [NonSerialized] private float _currentHP;
    [SerializeField][Range(0f, 40f)] private float _maxArmor;
    [NonSerialized] private float _currentArmor;
    [SerializeField][Min(0.1f)] private float _armorStartTimeRegen;
    [SerializeField][Min(0.1f)] private float _armorTickPeriodRegen;
    [NonSerialized] private bool _invincibility = false;
    [SerializeField][Range(0.1f, 40f)] private float _invincibilityPeriod;
    [NonSerialized] private static HashSet<ItemBehaviour> _playerItems;

    private void Awake()
    {
        _currentHP = _maxHP;
        _currentArmor = _maxArmor;
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

        while (_currentArmor < _maxArmor)
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

    public static void AddItem(ItemBehaviour item)
    {
        if (!_playerItems.Contains(item))
        {
            _playerItems.Add(item);
        }
        else
        {
            throw new ArgumentException($"Item {item.name} is already taken");
        }
    }
    public static void RemoveItem(ItemBehaviour item)
    {
        if (_playerItems.Contains(item))
        {
            _playerItems.Remove(item);
        }
        else
        {
            throw new ArgumentException($"Item {item.name} does not exists");
        }
    }
}
