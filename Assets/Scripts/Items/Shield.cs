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
        playerStats.JumpForce += 20;
    }

    public override void LoseEffect()
    {
        playerStats.JumpForce -= 20;
    }
}