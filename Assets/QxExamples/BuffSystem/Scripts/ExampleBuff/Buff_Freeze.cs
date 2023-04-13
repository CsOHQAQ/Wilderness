using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该buff为示例，用于展示在buff积累槽满后触发的效果
/// </summary>
public class Buff_Freeze : Buff
{
    public override void Init()
    {
        base.Init();
        data = new DataChanger();
        data.MoveSpeedMul._index = -99999;
        Debug.Log("冻住了！");
    }
    public override void ClearEffect()
    {
        base.ClearEffect();
    }

}
