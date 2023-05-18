using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class LoadSceneProcedure : ProcedureBase
{
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        Debug.Log("#Procedure正在尝试打开游戏场景");
        LoadSceneManager.Instance.OpenLevel("GamingScene", OnComplete);
    }

    protected override void OnLeave()
    {
        base.OnLeave();
    }

    void OnComplete(string str)
    {
        Debug.Log("完成场景加载");
        ProcedureManager.Instance.ChangeTo<GameProcedure>();
    }
}
