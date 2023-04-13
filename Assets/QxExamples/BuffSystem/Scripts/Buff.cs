using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class Buff 
{
    public float LastingTime;//持续时间
    public float Count;//积累槽，用于积累一定程度后触发效果的buff
    public float MaxCount;//积累槽上限，搭配上面那个食用
    public DataChanger data;

    protected BuffManager buffManager;
    public void SetManager(BuffManager bm)
    {
        buffManager = bm;
    }
    /// <summary>
    /// 初始化buff
    /// </summary>
    public virtual void Init()
    {
    }
    /// <summary>
    /// 每帧刷新buff
    /// </summary>
    public virtual void Refresh()
    {
        LastingTime -= Time.deltaTime;
        if (Count > 0)
        {
            Count -= Count / LastingTime * Time.deltaTime;
        }
    }
    /// <summary>
    /// 当积累槽积满后触发效果
    /// </summary>
    public virtual void ActivateWhenFull(){}
    /// <summary>
    /// 移除该buff前执行的操作
    /// </summary>
    public virtual void BeforeRemove()
    {
        ClearEffect();
    }
    /// <summary>
    /// 清除该buff效果
    /// </summary>
    public virtual void ClearEffect()
    {
    }

}

