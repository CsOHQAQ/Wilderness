using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class BuffSystemProcedure : ProcedureBase
{
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        GameObject go = new GameObject();
        go.name = "BuffEntity";
        go.AddComponent<Rigidbody2D>().gravityScale=0;
        go.AddComponent<SpriteRenderer>().sprite = ResourceManager.Instance.Load<Sprite>("Texture/Property/1001");
        go.AddComponent<BuffManager>();
        go.AddComponent<BuffSystemEntity>();
        UIManager.Instance.Open("Example_BuffTestUI");
    }
}
