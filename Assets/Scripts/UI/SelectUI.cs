using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QxFramework.Core;
public class SelectUI : UIBase
{
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        Get<Button>("Level1").onClick.SetListener(ChangeLv1);
        Get<Button>("Level2").onClick.SetListener(ChangeLv2);
        
    }

    public void ChangeLv1()
    {
        Debug.Log("切换至关卡1");
        ProcedureManager.Instance.ChangeTo("GamingProcedure", "Level1");
    }
    public void ChangeLv2()
    {
        Debug.Log("切换至关卡2");
        ProcedureManager.Instance.ChangeTo("GamingProcedure", "Level2");
    }
}
