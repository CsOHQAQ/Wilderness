using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class GamingModule : Submodule
{
    string LevelName;
    public GamingModule(object args)
    {
        LevelName = (string)args;
    }
    protected override void OnInit()
    {
        base.OnInit();
        GameObject.Find("Player").GetComponent<Player>().canControl = true;
        GameObject.Find("Main Camera").transform.position = GameObject.Find(LevelName).transform.position + new Vector3(0, 0, -50);

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
