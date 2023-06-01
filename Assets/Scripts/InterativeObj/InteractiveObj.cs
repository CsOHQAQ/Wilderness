using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public abstract class InteractiveObj:MonoBehaviour
{
    public Vector2 postion;
    private SpriteRenderer sprite;
    public abstract void Init();

    public abstract void Interact();

    /// <summary>
    /// 尽量不要使用update进行更新
    /// </summary>
    public abstract void Refresh(GameDateTime jumpTime);
}
