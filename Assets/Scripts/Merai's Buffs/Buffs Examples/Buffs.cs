using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Buffs : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] public string Name;
    [SerializeField] public string Description;
    [SerializeField] protected float Value;
    [SerializeField] protected Sprite IconImage;


    [Header("UI")]
    public Text NameTxt;
    public Text DescriptionTxt;

    public Image Icon;
  

    protected virtual void Awake()
    {
        Init();
    }  
    
    private void Init()
    {
       NameTxt.text = Name;
       DescriptionTxt.text = Description;
       Icon.sprite = IconImage;
       Apply();
    }
    public abstract void Apply();

    public abstract void Discard();
}
