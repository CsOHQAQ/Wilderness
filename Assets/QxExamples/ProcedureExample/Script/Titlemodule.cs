using QxFramework.Core;
using UnityEngine;
public class Titlemodule : Submodule {

    protected override void OnInit()
    {
        base.OnInit();
        InitGame();
    }
    private void InitGame()
    {
        QXData.Instance.SetTableAgent();
        GameMgr.Instance.InitModules();
        UIManager.Instance.Open("Title");
        Debug.Log("进入标题流程");
    }
    //在切换流程时会将上一流程的子模块destory掉
    protected override void OnDestroy()
    {
        base.OnDestroy();
        UIManager.Instance.Close("Title");
    }
}
