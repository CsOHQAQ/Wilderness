using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : InteractiveObj
{
    public float buildingProgress;
    
    public override void Init(Object loadInstance = null)
    {
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    public override void Interact(PlayerBase player)
    {

    }
    public override void Refresh(GameDateTime jumpTime)
    {

    }
    public virtual void OnFinishBuild()
    {

    }
    public override void OnDestory()
    {
    }
}
