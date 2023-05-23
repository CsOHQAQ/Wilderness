using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcedure : ProcedureBase {

    private MapManager mapManager;
    private CameraController cam;
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);

        PlayerBase player = ResourceManager.Instance.Instantiate("Prefabs/Player/Player").GetComponent<PlayerBase>();
        player.Init();
        UIManager.Instance.Open("CreateMapHintUI");
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        mapManager.Init(player);
        UIManager.Instance.Close("CreateMapHintUI");
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
        Debug.Log("#Procedure试试看呢");
    }
}
