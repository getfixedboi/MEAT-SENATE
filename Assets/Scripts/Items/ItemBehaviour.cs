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
    protected override void Awake()
    {
        base.Awake();
        itemSprite = Resources.Load<Sprite>(GetType().Name);
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStatictics>();
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
