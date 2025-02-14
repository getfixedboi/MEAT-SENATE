using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBuff : Buffs
{

    protected override void Awake()
    {
        base.Awake();
    }
    public override void Apply()
    {
        PlayerStatictics.Instance.maxHP += Value;
    }

    public override void Discard()
    {
        PlayerStatictics.Instance.maxHP -= Value;
        Destroy(gameObject);
    }
}
