using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using QxFramework.Core;
public class GameOverUI : UIBase
{
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        Get<Button>("Button").onClick.AddListener(()=> {//切换至标题页面
            LoadSceneManager.Instance.CloseLevel();
            UIManager.Instance.CloseAll();
            ProcedureManager.Instance.ChangeTo<TitleProcedure>();
        });
    }
}
