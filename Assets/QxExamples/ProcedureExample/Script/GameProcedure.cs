using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcedure : ProcedureBase {

    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        PlayerBase player = ResourceManager.Instance.Instantiate("Prefabs/Player/Player").GetComponent<PlayerBase>();
        player.Init();
    }

    protected override void OnLeave()
    {
        base.OnLeave();
    }
    protected override void OnUpdate(float elapseSeconds)
    {
        base.OnUpdate(elapseSeconds);
        Debug.Log("#Procedure试试看呢");
    }
}
