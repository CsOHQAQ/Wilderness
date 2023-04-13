using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class SelectProcedure : ProcedureBase
{
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        AddSubmodule(new SelectModule());
    }
    protected override void OnLeave()
    {
        base.OnLeave();
    }
}
