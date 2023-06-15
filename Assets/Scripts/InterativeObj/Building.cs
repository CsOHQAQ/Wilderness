using System;
using System.Collections.Generic;
using UnityEngine.UI;
using QxFramework.Core;
using UnityEngine;

public class Building : InteractiveObj
{
    [HideInInspector]
    public string buildingName = "";//建筑名
    public GameDateTime needTime = new GameDateTime();//完成建筑所需的时间
    public GameDateTime startBuildTime = new GameDateTime();
    private UIBase buildProgressUI;//建筑进度的进度条
    public UIBase interactUI;
    public float buildProgress;//建筑进度

    public bool isFinishBuild = false;//是否完成建筑

    protected Action<HeatSpot> setHeatSpot;//向MapManager添加热源的函数

    //被生成的时候的初始化,子类需要在进入OnEnterBuild之前调用
    public override void Init(MapBlock block,UnityEngine.Object loadInstance = null)
    {
        mapBlock = block;
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        lastVisitTime = GameMgr.Get<IGameTimeManager>().GetNow();
        isFinishBuild = false;
        startBuildTime = GameMgr.Get<IGameTimeManager>().GetNow();
        buildProgress = 0;
    }
    
    /// <summary>
    /// 与建筑交互
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(PlayerBase player)
    {

    }
    /// <summary>
    /// 刷新建筑
    /// </summary>
    /// <param name="current">当前的时间</param>
    public override void Refresh(GameDateTime current)
    {
        lastVisitTime = current;
    }
    /// <summary>
    /// 每帧刷新
    /// </summary>
    private void Update()
    {
        if (!isFinishBuild)
        {
            if (buildProgressUI.isActiveAndEnabled)
            {

                buildProgressUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, sprite.size.y / 2 + 2));//刷新位置

                buildProgress = (float)(GameMgr.Get<IGameTimeManager>().GetNow() - startBuildTime).TotalMinutes / needTime.TotalMinutes;//刷新进度条
                buildProgressUI.GetComponent<Image>().fillAmount = buildProgress;
                if (buildProgress >= 1)
                {
                    OnFinishBuild();
                }
            }
        }
        if (interactUI != null && interactUI.isActiveAndEnabled)
        {
            interactUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, sprite.size.y / 2 + 1));
        }
    }
    /// <summary>
    /// 开始建造时调用一次
    /// </summary>
    public virtual void OnEnterBuild()
    {
        buildProgressUI = UIManager.Instance.Open("BuildProgressUI");
    }
    /// <summary>
    /// 完成建造时调用一次
    /// </summary>
    public virtual void OnFinishBuild()
    {
        isFinishBuild = true;
        UIManager.Instance.Close(buildProgressUI);
        interactable = true;
    }
    /// <summary>
    /// 被摧毁时调用一次
    /// </summary>
    public override void OnDestory()
    {
        LeaveInteractRange();
    }
    /// <summary>
    /// 成为当前可交互对象后显示（每帧刷新）
    /// </summary>
    public override void EnterInteractRange()
    {
        interactUI = UIManager.Instance.Open("InteractUI");
        interactUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, sprite.size.y / 2 + 5));
    }
    /// <summary>
    ///不再是当前可交互对象后显示（每帧刷新）
    /// </summary>
    public override void LeaveInteractRange()
    {

        UIManager.Instance.Close(interactUI);
        interactUI = null;
    }
    public void SetHeatSpot(Action<HeatSpot> addHeatSpot=null)
    {
        setHeatSpot = addHeatSpot;
    }
}
