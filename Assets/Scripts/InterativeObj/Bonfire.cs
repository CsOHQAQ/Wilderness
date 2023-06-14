using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class Bonfire : Building
{
    public float remainBurnMinute = 0f;
    public bool isBurn = false;
    public float basicTemperature = 80f;
    public float warmRange = 10f;


    public override void Init(MapBlock block, Object loadInstance = null)
    {
        base.Init(block, loadInstance);
        if (loadInstance != null)
        {
            //读取存档数据
        }

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
