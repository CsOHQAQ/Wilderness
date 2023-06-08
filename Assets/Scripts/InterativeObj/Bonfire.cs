using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class Bonfire : Building
{
    public float remainBurnTime = 0f;
    public bool isBurn = false;
    public float basicTemperature = 80f;
    public float warmRange = 10f;


    public override void Init(Object loadInstance = null)
    {
        base.Init(loadInstance);
    }
    public override void Interact(PlayerBase player)
    {
        base.Interact(player);
    }
    public override void Refresh(GameDateTime jumpTime)
    {
        base.Refresh(jumpTime);
    }
    public override void OnFinishBuild()
    {
        base.OnFinishBuild();
    }
    public override void OnDestory()
    {
        base.OnDestory();
    }

}
