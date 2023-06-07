using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class RenewableObj : InteractiveObj
{
     public GameDateTime lastVisitTime;
    public override void Init(Object loadInstance=null)
    {
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    public override void Interact(PlayerBase player)
    {
    }
    public override void Refresh(GameDateTime current)
    {
        lastVisitTime= current;

    }

    public override void OnDestory()
    {
    }
}
