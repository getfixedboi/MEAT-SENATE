using UnityEngine;

public class BisonSteak : ItemBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        SetDesc();
    }
    public override void GetEffect()
    {
        playerStats.maxHP += 20;
    }

    public override void LoseEffect()
    {
        playerStats.maxHP -= 20;
        if (playerStats.СurrentHP > playerStats.maxHP)
        {
            playerStats.СurrentHP = playerStats.maxHP;
        }
        playerStats.maxHP -= 0;//папина гордость, мамина радость
    }

    public override void SetDesc()
    {
        itemName = "Bison stake";
        itemDescription = "HP up";
    }
}