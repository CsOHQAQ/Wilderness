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
        bool isMoveX = false;
        bool isMoveY = false;
        if (InputManager.Instance.GetButton(InputEnum.Up)&&(!InputManager.Instance.GetButton(InputEnum.Down)))//向上移动
        {
            isMoveY = true;
            body.velocity = new Vector2(body.velocity.x, data.Velocity);
        }
        if(InputManager.Instance.GetButton(InputEnum.Down) && (!InputManager.Instance.GetButton(InputEnum.Up)))
        {
            isMoveY = true;
            body.velocity = new Vector2(body.velocity.x, -data.Velocity);
        }
        if (InputManager.Instance.GetButton(InputEnum.Left) && (!InputManager.Instance.GetButton(InputEnum.Right)))//向上移动
        {
            isMoveX = true;
            body.velocity = new Vector2(-data.Velocity, body.velocity.y);
        }
        if (InputManager.Instance.GetButton(InputEnum.Right) && (!InputManager.Instance.GetButton(InputEnum.Left)))//向上移动
        {
            isMoveX = true;
            body.velocity = new Vector2(data.Velocity, body.velocity.y);
        }
        if (!isMoveX)
        {
            body.velocity = new Vector2(Mathf.Max(0,body.velocity.x-data.Velocity/5), body.velocity.y);
        }
        if (!isMoveY)
        {
            body.velocity = new Vector2(body.velocity.x ,Mathf.Max(0, body.velocity.y - data.Velocity / 5));
        }
    }

    public void RefreshData()
    {

    }
}
