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
        playerStats.MaxHP += 5;
    }

    public override void LoseEffect()
    {
        playerStats.MaxHP -= 5;
        if (playerStats.СurrentHP > playerStats.MaxHP)
        {
            playerStats.СurrentHP = playerStats.MaxHP;
        }
    }
    public override void SetDesc()
    {
        itemName = "Breakfast";
        itemDescription = "HP up";
    }
}
