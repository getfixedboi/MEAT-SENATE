using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondModifier : ModifierBehaviour
{
    public override void SetDesc()
    {
        modifierName = "Second mod";
        modifierDescription = "Speed up";
    }

    public override void AttachProjectileEffect(GameObject projectile)
    {
        PlayerSkills.ProjectileExtraForce = 5f;
        projectile.GetComponent<Renderer>().material.color = Color.blue;
    }

    public override void DetachProjectileEffect()
    {
        PlayerSkills.ProjectileExtraForce = 0f;
    }
}
