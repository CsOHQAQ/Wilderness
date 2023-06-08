using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public abstract class InteractiveObj:MonoBehaviour
{
    protected private SpriteRenderer sprite;
    /// <summary>
    /// 每次被生成的时候的初始化
    /// </summary>
    public abstract void Init(Object loadInstance=null);

    public abstract void Interact(PlayerBase player);

    /// <summary>
    /// 尽量不要使用update进行更新
    /// </summary>
    public abstract void Refresh(GameDateTime current);
    public abstract void OnDestory();
    public abstract void EnterInteractRange();
    public abstract void LeaveInteractRange();

}
