using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcedure : ProcedureBase {

    private MapManager mapManager;
    private CameraController cam;
    private GlobalLightControl globalLight;
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        //重设游戏时间
        GameMgr.Get<IGameTimeManager>().ResetTime();
        //初始化玩家类
        PlayerBase player = ResourceManager.Instance.Instantiate("Prefabs/Player/Player").GetComponent<PlayerBase>();
        player.Init();
        //初始化地图类
        UIManager.Instance.Open("CreateMapHintUI");
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        mapManager.Init(player);
        UIManager.Instance.Close("CreateMapHintUI");
        //初始化全局光照
        globalLight = ResourceManager.Instance.Instantiate("Prefabs/GlobalLight").GetComponent<GlobalLightControl>();
        globalLight.Init();
        //初始化摄像机
        cam = Camera.main.GetComponent<CameraController>();
        cam.Init(player);
        cam.needFollow = true;

    }

    protected override void OnLeave()
    {
        base.OnLeave();
    }
    protected override void OnUpdate(float elapseSeconds)
    {
        base.OnUpdate(elapseSeconds);
    }
}
