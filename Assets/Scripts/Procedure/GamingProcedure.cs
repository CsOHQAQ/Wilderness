using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class GamingProcedure : ProcedureBase
{
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        AddSubmodule(new GamingModule(args));
    }
    protected override void OnLeave()
    {
        base.OnLeave();
    }
}
