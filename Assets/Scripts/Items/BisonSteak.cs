using UnityEngine;

public class BisonSteak : ItemBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        itemName = "Bison stake";
        itemDescription = "HP up";
    }
    public override void GetEffect()
    {
        playerStats.MaxHP += 20;
    }

    public override void LoseEffect()
    {
        playerStats.MaxHP -= 20;
        if (playerStats.СurrentHP > playerStats.MaxHP)
        {
            playerStats.СurrentHP = playerStats.MaxHP;
        }
    }
}