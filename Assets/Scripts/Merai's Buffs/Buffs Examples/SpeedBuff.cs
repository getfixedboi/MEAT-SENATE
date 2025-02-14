using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buffs
{
  protected override void Awake()
  {
    base.Awake();
  }
  public override void Apply()
  {
      PlayerStatictics.Instance.BaseMovSpeed += Value;
  }

  public override void Discard()
  {
    PlayerStatictics.Instance.BaseMovSpeed -= Value;
    Destroy(gameObject);
  }
}
