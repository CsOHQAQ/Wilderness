using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using System;

public class PlayerBase: MonoBehaviour
{
    public PlayerData data;
    [HideInInspector]
    public Rigidbody2D body;
    public bool canMove = true;
    [HideInInspector]
    public float interactRange = 2f;
    public float environmentTemp = 100f;

    private Action<PlayerBase> interactFunc;
    private PlayerStateUI stateUI;
    private BackPackUI backPackUI;

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
        stateUI = UIManager.Instance.Open("PlayerStateUI",2,"",this).GetComponent<PlayerStateUI>();
        backPackUI = UIManager.Instance.Open("BackPackUI",2,"",new CargoData[] { data.backpack } ).GetComponent<BackPackUI>();
    }

    /// <summary>
    /// 供外部调用的交互
    /// </summary>
    /// <param name="action"></param>
    public void SetInteractFunc(Action<PlayerBase> action)
    {
        interactFunc = action;
    }


    private void Update()
    {
        Move();
        Interact();
        data.RefreshData(environmentTemp);
    }
    private void OnDestroy()
    {
        UIManager.Instance.Close(stateUI);
        UIManager.Instance.Close(backPackUI);
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

    public void Interact()
    {
        if (InputManager.Instance.GetButtonDown(InputEnum.Interact))
        {
            interactFunc.Invoke(this);
        }
    }
}
