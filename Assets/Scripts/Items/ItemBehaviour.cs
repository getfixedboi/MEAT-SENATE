using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ItemBehaviour : Interactable
{
    protected static PlayerStatictics playerStats;
    protected Sprite itemSprite;//название спрайта должно быть точно таким же как и название класса(предмета) для которого спрайт нужен
    protected string itemName;
    protected string itemDescription;
    protected Rigidbody rb;
    protected bool canBeStopped = false;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        itemSprite = Resources.Load<Sprite>($"Sprites/{GetType().Name}");
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStatictics>();
    }
    protected void Start()
    {
        StartCoroutine(C_DisableCollider());
    }
    protected void Update()
    {
        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        if (rb.velocity == Vector3.zero && canBeStopped)
        {
            transform.Rotate(0, 0.1f, 0);
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
    private IEnumerator C_DisableCollider()
    {
        yield return new WaitForSeconds(.5f);
        canBeStopped = true;
    }
    protected void OnEnable()
    {
        rb.isKinematic = false;
        rb.useGravity = true;

        canBeStopped = false;
        StartCoroutine(C_DisableCollider());
    }
    protected void OnDisable()
    {
        StopCoroutine(C_DisableCollider());
    }
    protected void OnGet()
    {
        playerStats.AddItem(this);
        GetEffect();
    }
    public void OnGet(ItemBehaviour item)
    {
        playerStats.AddItem(item);
        item.GetEffect();
    }
    public void OnDrop()
    {
        playerStats.RemoveItem(this);
        LoseEffect();
    }
    public void OnDrop(ItemBehaviour item)
    {
        playerStats.RemoveItem(item);
        item.LoseEffect();
    }
    public override sealed void OnFocus()
    {
        InteractText = $"{GetDesc()}" + $"\n\n[E] - Take";
    }
    public override sealed void OnLoseFocus()
    {
        InteractText = "";
    }
    public override sealed void OnInteract()
    {
        try
        {
            OnGet();
        }
        catch (ArgumentException)
        {
            OnDrop();
        }
    }
    public abstract void GetEffect();
    public abstract void LoseEffect();
    public string GetDesc()
    {
        return $"{itemName}" + $"\n{itemDescription}";
    }
    public Sprite GetSprite()
    {
        return itemSprite;
    }
}
