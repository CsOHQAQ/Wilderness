using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class SelectModule : Submodule
{
    protected override void OnInit()
    {
        base.OnInit();
        GameObject.Find("Player").GetComponent<Player>().canControl = false;
        GameObject.Find("Main Camera").transform.position = new Vector3(0, 0, -50);
        UIManager.Instance.Open("SelectUi");
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        UIManager.Instance.Close("SelectUi");
    }
}
