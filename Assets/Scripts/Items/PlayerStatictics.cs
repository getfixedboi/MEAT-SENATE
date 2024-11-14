using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

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
    private float MaxHP = 100;

    public float maxHP
    {
        get => MaxHP;
        set
        {
            MaxHP = value;
            _healthText.text = $"{СurrentHP}/{MaxHP}";
        }
    }

    public float СurrentHP;

    private bool _invincibility = false;

    [SerializeField]
    [Range(0.1f, 40f)]
    private float _invincibilityPeriod;

    private static HashSet<ItemBehaviour> _playerItems = new();
    private static HashSet<ModifierBehaviour> _playerModifiers = new();
    #endregion

    [Header("UI")]

    [SerializeField] private Canvas _playerCanvas;
    [SerializeField] private GameObject _itemImagePrefab;
    [SerializeField] private GameObject _modifierImagePrefab;
    [SerializeField] private UnityEngine.UI.Text _healthText;

    private static List<GameObject> _itemUiPrefabs = new();
    private static List<GameObject> _modifierUiPrefabs = new();

    public static ShowModifierDescOnUI CurrentModifier;

    public GameObject ItemGrid;
    public GameObject ModGrid;

    public static GameObject Instance;

    private void Awake()
    {
        Instance = this.gameObject;
        СurrentHP = MaxHP;
        _healthText.text = $"{СurrentHP}/{MaxHP}";
    }

    private void Update()
    {
        // Логика обновления, если необходима
    }

    public void Heal(float healAmount)
    {
        СurrentHP += healAmount;
        if (СurrentHP > MaxHP)
        {
            СurrentHP = MaxHP;
        }
        _healthText.text = $"{СurrentHP}/{MaxHP}";
    }

    public void TakeDamage(float inflictedDamage)
    {
        if (_invincibility) return;

        СurrentHP -= inflictedDamage;
        PlayerProgress.ReceivedDamage += inflictedDamage;
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
            AddItemUI(item);
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
            if (param) item.gameObject.GetComponent<MeshRenderer>().enabled = false;

            if (!item.isShop)
            {
                TakeFromGroundItem(item.gameObject, param);
            }
            else
            {
                item.isShop = false;
                item.transform.SetParent(transform);
                item.transform.localPosition = Vector3.zero;

                if (param) item.gameObject.GetComponent<MeshRenderer>().enabled = true;

                if (item.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.isKinematic = true;
                if (item.TryGetComponent<Collider>(out Collider col)) col.enabled = false;

                item.gameObject.SetActive(false);
            }
            _playerItems.Add(item);
            AddItemUI(item);
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
            RemoveItemUI(item);
            ThrowAwayItem(item.gameObject);
        }
        else
        {
            throw new ArgumentException($"Item {item.name} does not exist");
        }
    }

    private void AddItemUI(ItemBehaviour item)
    {
        GameObject gameObj = Instantiate(_itemImagePrefab, _playerCanvas.transform.position, Quaternion.identity);

        var showItemDesc = gameObj.GetComponent<ShowItemDescOnUI>();
        if (showItemDesc != null)
        {
            showItemDesc.Canvas = _playerCanvas;
            showItemDesc.ItemRef = item.GetComponent<ItemBehaviour>();
        }

        gameObj.transform.SetParent(ItemGrid.transform, false);

        _itemUiPrefabs.Add(gameObj);
    }

    private void RemoveItemUI(ItemBehaviour item)
    {
        List<ItemBehaviour> list = _playerItems.ToList();

        GameObject tempItem = _itemUiPrefabs[list.IndexOf(item)];

        _itemUiPrefabs.Remove(tempItem);
        _playerItems.Remove(item);

        Destroy(tempItem);
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
            if (param) item.gameObject.GetComponent<MeshRenderer>().enabled = false;

            TakeFromGroundItem(item.gameObject, param);
            _playerModifiers.Add(item);
            ReloadModifierUI();
        }
        else
        {
            throw new ArgumentException($"Modifier {item.name} is already taken");
        }
    }

    private void ReloadModifierUI()
    {
        for (int i = 0; i < _modifierUiPrefabs.Count; i++)
        {
            Destroy(_modifierUiPrefabs[i]);
        }

        _modifierUiPrefabs.Clear();

        foreach (ModifierBehaviour mod in _playerModifiers)
        {
            GameObject gameObj = Instantiate(_modifierImagePrefab, _playerCanvas.transform.position, Quaternion.identity);

            var modRef = mod.GetComponentInChildren<ModifierBehaviour>();
            gameObj.GetComponent<ShowModifierDescOnUI>().ModRef = modRef;

            var showModDesc = gameObj.GetComponent<ShowModifierDescOnUI>();
            if (showModDesc != null)
            {
                showModDesc.Canvas = _playerCanvas;
            }

            gameObj.transform.SetParent(ModGrid.transform, false);

            _modifierUiPrefabs.Add(gameObj);

            if (modRef == null) Debug.Log("Не удалось скопировать ссылку на компонент");
        }
    }
    #endregion

    private void TakeFromGroundItem(GameObject item)
    {
        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.isKinematic = true;
        if (item.TryGetComponent<Collider>(out Collider col)) col.enabled = false;

        StartCoroutine(AnimateItemAbsorption(item));
    }

    private void TakeFromGroundItem(GameObject item, bool param)
    {
        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.isKinematic = true;
        if (item.TryGetComponent<Collider>(out Collider col)) col.enabled = false;

        StartCoroutine(AnimateItemAbsorption(item, param));
    }

    private IEnumerator AnimateItemAbsorption(GameObject item)
    {
        float speed = 20f;
        Vector3 targetPosition = transform.position + transform.forward * 0.5f - transform.up * .4f;

        while (Vector3.Distance(item.transform.position, targetPosition) > 0.1f)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        item.transform.position = targetPosition;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        item.SetActive(false);
    }

    private IEnumerator AnimateItemAbsorption(GameObject item, bool param)
    {
        float speed = 20f;
        Vector3 targetPosition = transform.position + transform.forward * 0.5f - transform.up * .4f;

        while (Vector3.Distance(item.transform.position, targetPosition) > 0.1f)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        item.transform.position = targetPosition;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;

        if (param) item.gameObject.GetComponent<MeshRenderer>().enabled = true;

        item.SetActive(false);
    }

    private void ThrowAwayItem(GameObject item)
    {
        item.SetActive(true);

        item.transform.SetParent(null);

        item.transform.position = transform.position + transform.forward;

        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
        }

        StartCoroutine(EnableColliderAfterDelay(item, 0.1f));
    }

    private IEnumerator EnableColliderAfterDelay(GameObject item, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (item.TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = true;
        }
    }

    private void Debugging(string startMes)
    {
        Debug.Log(startMes);

        foreach (var v in _playerItems)
        {
            Debug.LogWarning(v.name);
        }
    }
}
