using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该buff为示例，用于展示有积累槽，并且会根据积累槽影响系数
/// </summary>
public class Buff_Cold : Buff
{
    public Buff_Cold()
    {
        MaxCount = 100;

        data = new DataChanger();
    }
    public override void Refresh()
    {
        base.Refresh();
        data.MoveSpeedMul._index = -1 * (Count / MaxCount);
    }
    public override void ActivateWhenFull()
    {
        base.ActivateWhenFull();
        buffManager.AddBuff(new Buff_Freeze(), 2);
        buffManager.RemoveBuff(this);
    }
}
