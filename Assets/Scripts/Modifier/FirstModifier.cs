using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstModifier : ModifierBehaviour
{
    public override void SetDesc()
    {
        modifierName = "First mod";
        modifierDescription = "DMG up";
    }

    public override void AttachProjectileEffect(GameObject projectile)
    {
        projectile.GetComponent<PlayerProjectile>().Damage += 13;
        projectile.GetComponent<Renderer>().material.color = Color.red;
    }

    public override void DetachProjectileEffect() { }
}
