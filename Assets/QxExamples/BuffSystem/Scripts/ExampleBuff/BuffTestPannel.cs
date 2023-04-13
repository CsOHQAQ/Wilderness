using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using QxFramework.Core;
public class BuffTestPannel : UIBase
{
    private BuffSystemEntity go;
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        go = GameObject.Find("BuffEntity").GetComponent<BuffSystemEntity>();
        Get<Button>("AddRunBtn").onClick.AddListener(() =>
        {
            go.buffManager.AddBuff(new Buff_Run(), 1);
        });
        Get<Button>("AddColdBtn").onClick.AddListener(() =>
        {
            go.buffManager.AddBuff(new Buff_Cold(), 3,20);
        });
        Get<Button>("ClearBuff").onClick.AddListener(() =>
        {
            go.buffManager.RemoveAllBuff();
        }); 
        Get<Button>("ResetBtn").onClick.AddListener(() =>
        {
            go.transform.position = new Vector3(0, 0, 0);
        });
    }
}
