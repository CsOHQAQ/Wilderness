using System.Collections;
using System.Collections.Generic;
using QxFramework.Core;
using UnityEngine;

public class Building : InteractiveObj
{
    [HideInInspector]
    public string buildingName = "";
    public Dictionary<Item, int> needItems = new Dictionary<Item, int>();
    public GameDateTime needTime = new GameDateTime();

    private UIBase buildProgressUI;
    public UIBase interactUI;
    public float buildingProgress;
    
    //被创建的时候的初始化
    public override void Init(MapBlock block, Object loadInstance = null)
    {
        mapBlock = block;
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        lastVisitTime = GameMgr.Get<IGameTimeManager>().GetNow();
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
    /// <param name="jumpTime">自上一次刷新过去了多久</param>
    public override void Refresh(GameDateTime jumpTime)
    {

    }
    /// <summary>
    /// 每帧刷新
    /// </summary>
    private void Update()
    {
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
    }
    /// <summary>
    /// 完成建造时调用一次
    /// </summary>
    public virtual void OnFinishBuild()
    {

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
        
    }
}
