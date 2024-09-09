using UnityEngine;

public class Shield : ItemBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        SetDesc();
    }
    public override void GetEffect()
    {
        playerStats.JumpForce += 10;
    }

    public override void LoseEffect()
    {
        playerStats.JumpForce -= 10;
    }
    public override void SetDesc()
    {
        itemName = "Shield";
        itemDescription = "Shield up";
    }
}