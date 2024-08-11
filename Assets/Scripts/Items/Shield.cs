using UnityEngine;

public class Shield : ItemBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        itemName = "Shield";
        itemDescription = "Shield up";
    }
    public override void GetEffect()
    {
        playerStats.MaxArmor += 20;
    }

    public override void LoseEffect()
    {
        playerStats.MaxArmor -= 20;
    }
}