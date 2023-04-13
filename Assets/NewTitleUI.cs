using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using QxFramework.Core;

public class NewTitleUI : UIBase
{
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        Get<Button>("StartButton").onClick.SetListener(() =>{ ProcedureManager.Instance.ChangeTo("SelectProcedure"); });
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
}
