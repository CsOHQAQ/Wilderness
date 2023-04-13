using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class StartProcedure: ProcedureBase
{
    //进入流程时调用
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        AddSubmodule(new Titlemodule());

    }
    //离开流程时调用
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
