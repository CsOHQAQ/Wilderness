using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QxFramework.Core;
public class ExitMenuUI : UIBase
{
    private PlayerBase player;
    /// <summary>
    /// 三个按钮，分别对应返回游戏、返回标题、退出游戏
    /// </summary>
    /// <param name="args"></param>
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        player = (PlayerBase)args;
        Get<Button>("ReturnBtn").onClick.AddListener(()=> {
            UIManager.Instance.Close(this);
        });
        Get<Button>("TitleBtn").onClick.AddListener(() => {
            LoadSceneManager.Instance.CloseLevel();
            UIManager.Instance.CloseAll();
            ProcedureManager.Instance.ChangeTo<TitleProcedure>();

        });
        Get<Button>("ExitBtn").onClick.AddListener(() => {
            Application.Quit();
        });
    }
    protected override void OnClose()
    {
        base.OnClose();
        player.isInteracting = false;
    }
}
