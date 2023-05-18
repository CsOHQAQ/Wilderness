using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using UnityEngine.UI;
public class TitleUI : UIBase
{

    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        Get<Button>("StartBtn").onClick.SetListener(StartGame);
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    void StartGame()
    {
        ProcedureManager.Instance.ChangeTo<LoadSceneProcedure>();
    }

}
