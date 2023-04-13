using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该buff为示例，用于展示无积累槽的buff
/// </summary>
public class Buff_Run : Buff
{
    public Buff_Run()
    {
        data = new DataChanger();
        data.MoveSpeedMul = new AddIndex(2f);

    }
    public override void Init()
    {
        base.Init();

    }
    public override void ClearEffect()
    {
        base.ClearEffect();
    }
    public override void Refresh()
    {
        base.Refresh();
    }
    
}
