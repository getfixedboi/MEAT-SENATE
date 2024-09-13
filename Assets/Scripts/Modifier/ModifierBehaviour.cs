using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public abstract class ModifierBehaviour : Interactable
{
    protected static PlayerStatictics playerStats;
    /// <summary>
    /// Название спрайта должно быть точно таким же как и название класса(модификатора) для которого спрайт нужен
    /// </summary>
    protected Sprite modifierSprite;
    protected string modifierName;
    protected string modifierDescription;
    protected Rigidbody rb;
    protected bool canBeStopped = false;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        modifierSprite = Resources.Load<Sprite>($"Sprites/{GetType().Name}");
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
    public void OnGet()
    {
        playerStats.AddModifier(this);
    }
    public void OnGet(ModifierBehaviour item)
    {
        playerStats.AddModifier(item);
    }
    public void OnGet(bool param)
    {
        playerStats.AddModifier(this,param);
    }
    public void OnDrop()
    {
        playerStats.RemoveModifier(this);
    }
    public void OnDrop(ModifierBehaviour item)
    {
        playerStats.RemoveModifier(item);
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
    public abstract void SetDesc();
    public abstract void AttachProjectileEffect(GameObject projectile);
    public abstract void DetachProjectileEffect();
    public string GetDesc()
    {
        SetDesc();
        return $"{modifierName}" + $"\n{modifierDescription}";
    }
    public Sprite GetSprite()
    {
        if (modifierSprite == null)
        {
            modifierSprite = Resources.Load<Sprite>($"Sprites/{GetType().Name}");
        }
        return modifierSprite;
    }
}
