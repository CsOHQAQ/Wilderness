using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using QxFramework.Core;

public class Bonfire : Building
{
    public float remainBurnMinute = 0f;//还能燃烧的分钟数
    public bool isBurn = false;
    public float basicTemperature = 80f;//开始燃烧之后的基础温度
    public float warmRange = 10f;//温度最远传播距离
    private float lightIntensity = 1;
    private Light2D light;


    public override void Init(MapBlock block, Object loadInstance = null)
    {
        base.Init(block, loadInstance);
        if (loadInstance != null)
        {
            //读取存档数据
        }
        light = GetComponent<Light2D>();
        light.intensity = lightIntensity;
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
