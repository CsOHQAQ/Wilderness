using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;


public class PlayerBase: MonoBehaviour
{
    public PlayerData data;
    [HideInInspector]
    public Rigidbody2D body;
    public bool canMove = true;
    public void Init(bool load=false)
    {
        body = GetComponent<Rigidbody2D>();
        if (load)
        {
            //]这里做存档读取
            return;
        }
            
        data = new PlayerData();
        data.Init();
        canMove = true;
    }
    private void Update()
    {
        Move();
    }

    public void Move()
    {
        bool isMove = false;
        Debug.Log("#Player 正在检测移动");
        if (InputManager.Instance.GetButton(InputEnum.Up)&&(!InputManager.Instance.GetButton(InputEnum.Down)))//向上移动
        {
            isMove = true;
            body.velocity = new Vector2(body.velocity.x, data.Velocity);
        }
        if(InputManager.Instance.GetButton(InputEnum.Down) && (!InputManager.Instance.GetButton(InputEnum.Up)))
        {
            isMove = true;
            body.velocity = new Vector2(body.velocity.x, -data.Velocity);
        }
        if (InputManager.Instance.GetButton(InputEnum.Left) && (!InputManager.Instance.GetButton(InputEnum.Right)))//向上移动
        {
            isMove = true;
            body.velocity = new Vector2(-data.Velocity, body.velocity.y);
        }
        if (InputManager.Instance.GetButton(InputEnum.Right) && (!InputManager.Instance.GetButton(InputEnum.Left)))//向上移动
        {
            isMove = true;
            body.velocity = new Vector2(data.Velocity, body.velocity.y);
        }
        if (!isMove)
        {
            body.velocity = new Vector2(Mathf.Max(0,body.velocity.x-data.Velocity/5), Mathf.Max(0, body.velocity.y - data.Velocity / 5));
        }
    }

    public void RefreshData()
    {

    }
}
