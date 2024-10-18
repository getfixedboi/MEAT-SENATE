using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    [HideInInspector] public string InteractText;
    /// <summary>
    /// Была ли последняя интеракия с объектом
    /// </summary>
    public bool IsLastInteracted { get; protected set; }
    // [SerializeField] protected AudioClip clip;
    // [SerializeField] protected AudioClip errorSound;
    // protected AudioSource source;
    protected virtual void Awake()
    {
        IsLastInteracted = false;
        //source = GetComponent<AudioSource>();
    }
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
    public abstract void OnInteract();
}
