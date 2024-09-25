using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatBeggar : Interactable
{
    [SerializeField] private List<ItemBehaviour> _itemShopPool;
    [SerializeField] private List<ShopItem> _shopDeals;
    [Space][SerializeField] private GameObject _shopCanvas;
    [SerializeField] private List<GameObject> _interactCanvasObjects;
    [SerializeField] private UnityEngine.UI.Text _itemDescriptionText;
    [SerializeField] private UnityEngine.UI.Text _meatCounter;
    private PlayerCameraMovement _playerCameraComponent;
    private PlayerSkills _playerSkillsComponent;
    public static bool IsShopping = false;
    protected override void Awake()
    {
        base.Awake();

        IsShopping = false;
        _meatCounter.text = PlayerSkills.MeatPieceCount.ToString();

        GameObject player = GameObject.FindWithTag("Player");
        _playerCameraComponent = player.GetComponentInChildren<PlayerCameraMovement>();
        _playerSkillsComponent = player.GetComponent<PlayerSkills>();

        _shopCanvas.SetActive(false);
        for (int i = 0; i < _interactCanvasObjects.Count; i++)
        {
            _interactCanvasObjects[i].SetActive(true);
        }
    }
    private void Start()
    {
        SetDeals();

        for (int i = 0; i < _shopDeals.Count; i++)
        {
            _shopDeals[i].MeatCounterText = _meatCounter;
        }
    }
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) { return; }

        if (_shopCanvas.activeSelf)
        {
            SwitchComponents(false);
            OnLoseFocus();
        }
    }
    public override void OnFocus()
    {
        if (!IsLastInteracted)
        {
            InteractText = "[E] - Trade";
        }
    }

    public override void OnInteract()
    {
        SwitchComponents(true);
    }

    public override void OnLoseFocus()
    {
        int buyedDeals = 0;
        foreach (ShopItem obj in _shopDeals)
        {
            if (obj != null)
            {
                break;
            }
            else
            {
                buyedDeals++;
            }
        }
        if (buyedDeals == 3)
        {
            IsLastInteracted = true;
        }
        SwitchComponents(false);
        InteractText = "";
    }
    private void SwitchComponents(bool value)
    {
        IsShopping = value;
        _shopCanvas.SetActive(value);

        for (int i = 0; i < _interactCanvasObjects.Count; i++)
        {
            _interactCanvasObjects[i].SetActive(!value);
        }

        _playerCameraComponent.enabled = !value;
        _playerSkillsComponent.enabled = !value;

        Cursor.lockState = value ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = value;
    }

    [ContextMenu("Set Deals")]
    private void SetDeals()
    {
        List<int> itemRandomIndex = new(3);

        if (_itemShopPool.Count < 3)
        {
            UnityEngine.Debug.LogError("Not enough items in the shop pool.");
            return;
        }

        while (itemRandomIndex.Count < 3)
        {
            int newIndex = UnityEngine.Random.Range(0, _itemShopPool.Count);
            if (!itemRandomIndex.Contains(newIndex))
            {
                itemRandomIndex.Add(newIndex);
            }
        }

        for (int i = 0; i < _itemShopPool.Count; i++)
        {
            _shopDeals[i].ItemRef = _itemShopPool[i].GetComponent<ItemBehaviour>();
            _shopDeals[i].ItemDescriptionText = _itemDescriptionText;
        }
    }
}
