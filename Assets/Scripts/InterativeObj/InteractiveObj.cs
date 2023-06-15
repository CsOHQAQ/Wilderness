using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public abstract class InteractiveObj:MonoBehaviour
{
    protected private SpriteRenderer sprite;
    protected private MapBlock mapBlock;//所在区块
    public GameDateTime lastVisitTime;//最后刷新时间
    public bool interactable = true;//能否被交互
    /// <summary>
    /// 每次被生成的时候的初始化
    /// </summary>
    public abstract void Init(MapBlock block,Object loadInstance=null);

    public abstract void Interact(PlayerBase player);

    /// <summary>
    /// 按照游戏内时间进行刷新
    /// </summary>
    public abstract void Refresh(GameDateTime current);
    public abstract void OnDestory();
    public abstract void EnterInteractRange();
    public abstract void LeaveInteractRange();

}
