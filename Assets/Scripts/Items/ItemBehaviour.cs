using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBehaviour : Interactable
{
    protected Sprite itemSprite;//название спрайта должно быть точно таким же как и название класса(предмета) для которого спрайт нужен
    protected string itemDescription;
    protected override void Awake()
    {
        base.Awake();
        itemSprite = Resources.Load<Sprite>(this.GetType().Name);
    }
    protected virtual void OnGet()
    {
        PlayerStatictics.AddItem(this);
    }
    protected virtual void OnDrop()
    {
        PlayerStatictics.RemoveItem(this);
    }
    public override sealed void OnFocus()
    {
        InteractText = "[E] - pick up";
    }
    public override sealed void OnLoseFocus()
    {
        InteractText = "";
    }
    public override sealed void OnInteract()
    {
        OnGet();
        OnLoseFocus();
        Destroy(gameObject);
    }
}
