using QxFramework.Core;
using UnityEngine;
public class Titlemodule : Submodule {
    private UIBase title;
    protected override void OnInit()
    {
        base.OnInit();
        InitGame();
    }
    private void InitGame()
    {
        QXData.Instance.SetTableAgent();
        GameMgr.Instance.InitModules();
        title= UIManager.Instance.Open("TitleUI");
    }
    //在切换流程时会将上一流程的子模块destory掉
    protected override void OnDestroy()
    {
        base.OnDestroy();
        UIManager.Instance.Close(title);
        Debug.Log("正在关闭标题ui");
    }
}
