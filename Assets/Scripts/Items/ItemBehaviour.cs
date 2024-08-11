using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBehaviour : Interactable
{
    protected static PlayerStatictics playerStats;
    protected Sprite itemSprite;//название спрайта должно быть точно таким же как и название класса(предмета) для которого спрайт нужен
    protected string itemName;
    protected string itemDescription;
    private bool testBool = false;
    protected override void Awake()
    {
        base.Awake();
        itemSprite = Resources.Load<Sprite>($"{GetType().Name}.png");
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStatictics>();
    }
    protected void OnGet()
    {
        playerStats.AddItem(this);
        GetEffect();
    }
    protected void OnGet(ItemBehaviour item)
    {
        playerStats.AddItem(item);
        item.GetEffect();
    }
    protected void OnDrop()
    {
        playerStats.RemoveItem(this);
        LoseEffect();
    }
    protected void OnDrop(ItemBehaviour item)
    {
        playerStats.RemoveItem(item);
        item.LoseEffect();
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
        if (testBool)
        {
            testBool=false;
            OnDrop();
        }
        else
        {
            testBool=true;
            OnGet();
        }
        //OnLoseFocus();
        //Destroy(gameObject);
    }
    public abstract void GetEffect();
    public abstract void LoseEffect();
    public string GetDesc()
    {
        return $"{itemName} +\n {itemDescription}";
    }
    public Sprite GetSprite()
    {
        return itemSprite;
    }
}
