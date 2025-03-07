﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using QxFramework.Core;

public class Bonfire : Building
{
    public float remainBurnMinute = 0f;//还能燃烧的分钟数
    public HeatSpot heatSpot;//由这个实例控制的热源
    private float baseHeat = 90f;//热源的基础热度
    private float lightIntensity = 1;//火焰的基础光照强度
    private Light2D fireLight;


    public override void Init(MapBlock block, Object loadInstance = null)
    {
        base.Init(block, loadInstance);
        if (loadInstance != null)
        {
            //读取存档数据
        }
        //初始化部分数据
        fireLight = GetComponent<Light2D>();
        fireLight.intensity = 0;
        needTime = new GameDateTime();
        needTime.Minutes = 30;
        remainBurnMinute = 0f;
        heatSpot = new HeatSpot();
        heatSpot.pos = transform.position;
        heatSpot.warmRange = 10f;
        interactable = false;
    }

    public override void Interact(PlayerBase player)
    {
        base.Interact(player);
        if (player.backPackUI.GetCurrentItem() != null)
        {
            if (player.backPackUI.GetCurrentItem().item.ItemCodeName == "Wood")//只有玩家当前选中的道具为木头才能给篝火添加燃料
            {
                GameMgr.Get<IItemManager>().RemoveItem(player.backPackUI.GetCurrentItem().CurrentPosID, 1, new CargoData[] { player.data.backpack });
                remainBurnMinute += 60f;
            }
        }
    }
    public override void Refresh(GameDateTime current)
    {
        if (remainBurnMinute > 0)//减少剩余燃烧时间
        {
            remainBurnMinute -= (current - lastVisitTime).TotalMinutes;
        }
        else
        {
            remainBurnMinute = 0;
        }
        //更新光照的强度
        lightIntensity = BurnTimeToLightIntensity();
        fireLight.intensity = lightIntensity;
        heatSpot.heat = baseHeat * lightIntensity;
        base.Refresh(current);
    }
    public override void OnFinishBuild()
    {
        setHeatSpot.Invoke(heatSpot);
        base.OnFinishBuild();
    }
    public override void OnDestory()
    {
        base.OnDestory();
    }
    private float BurnTimeToLightIntensity()
    {
        float declineTime = 60f;//亮度开始衰减时的亮度
        if (remainBurnMinute >= declineTime)
            return 1f;
        return remainBurnMinute / declineTime;
    }

}