using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBuff : Buffs
{
     protected override void Awake()
    {
        base.Awake();
    }
    public override void Apply()
    {
        PlayerStatictics.Instance.JumpForce += Value;
    }

    public override void Discard()
    {
        PlayerStatictics.Instance.JumpForce -= Value;
        Destroy(gameObject);
    }


}
