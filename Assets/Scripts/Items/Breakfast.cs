using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakfast : ItemBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        SetDesc();
    }
    public override void GetEffect()
    {
        playerStats.maxHP += 5;
    }

    public override void LoseEffect()
    {
        playerStats.maxHP -= 5;
        if (playerStats.СurrentHP > playerStats.maxHP)
        {
            playerStats.СurrentHP = playerStats.maxHP;
        }
        playerStats.maxHP -= 0;///папина гордость, мамина радость(активация свойства) 
    }
    public override void SetDesc()
    {
        itemName = "Breakfast";
        itemDescription = "HP up";
    }
}
