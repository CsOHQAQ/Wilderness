using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using System;

public class PlayerBase: MonoBehaviour
{
    //玩家所带实例
    public PlayerData data;
    //玩家GO的刚体
    [HideInInspector]
    public Rigidbody2D body;

    public bool canMove = true;
    [HideInInspector]
    public float interactRange = 2f;//玩家的交互距离

    public float environmentTemp = 100f;//感受到的环境温度

    public bool isInteracting = false;//是否正在交互

    public Action<PlayerBase> interactFunc;//玩家当前的交互事件
    public Action<PlayerBase, string> buildFunc;//玩家当前的建筑事件
    
    //一些由playerBase打开的UI
    private CraftTableUI craftTable;
    private PlayerStateUI stateUI;
    public BackPackUI backPackUI;

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

    private void Update()
    {
        //检测玩家的按键输入
        OpenExitMenu();
        OpenCraftTable();
        Move();
        Interact();
        UseItem();
        data.RefreshData(environmentTemp);

        if (data.Health <= 0)//如果生命值过低，即销毁玩家，并打开重开的页面
        {
            UIManager.Instance.Open("GameOverUI");
            OnDestroy();
            GameObject.Destroy(this);
        }

    }
    private void OnDestroy()
    {
        UIManager.Instance.Close(stateUI);
        UIManager.Instance.Close(backPackUI);
    }
    /// <summary>
    /// 
    /// </summary>
    public void Move()
    {
        bool isMoveX = false;
        bool isMoveY = false;
        if (!isInteracting)//根据按键分别相对应方向移动
        {
            if (InputManager.Instance.GetButton(InputEnum.Up) && (!InputManager.Instance.GetButton(InputEnum.Down)))//向上移动
            {
                isMoveY = true;
                body.velocity = new Vector2(body.velocity.x, data.Velocity);
            }
            if (InputManager.Instance.GetButton(InputEnum.Down) && (!InputManager.Instance.GetButton(InputEnum.Up)))
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
        }
        if (!isMoveX)//若x轴/y轴没任何按键让其移动，则减少对应方向上的速度。
        {
            body.velocity = new Vector2(Mathf.Max(0,body.velocity.x-data.Velocity/5), body.velocity.y);
        }
        if (!isMoveY)
        {
            body.velocity = new Vector2(body.velocity.x ,Mathf.Max(0, body.velocity.y - data.Velocity / 5));
        }
    }
    /// <summary>
    /// 检查玩家是否按下交互键
    /// </summary>
    public void Interact()
    {
        if (InputManager.Instance.GetButtonDown(InputEnum.Interact)&&!isInteracting)
        {
            interactFunc.Invoke(this);
        }
    }

    /// <summary>
    /// 检查玩家是否按下合成键
    /// </summary>
    public void OpenCraftTable()
    {
        if (InputManager.Instance.GetButtonDown(InputEnum.Craft)&&!isInteracting)
        {
            isInteracting = true;
            craftTable= UIManager.Instance.Open("CraftTableUI",2,"CraftTableUI",this).GetComponent<CraftTableUI>();
        }
    }
    /// <summary>
    /// 检查玩家是否要使用道具
    /// </summary>
    public void UseItem()
    {
        if (InputManager.Instance.GetButtonDown(InputEnum.UseItem)&&!isInteracting)
        {
            if (data.backpack.itemPiles.Count>0&& backPackUI.GetCurrentItem().item.ItemFunc != "")
            {
                if (backPackUI.GetCurrentItem().item.ItemType == ItemType.Building)//如果是建筑物就调用建筑函数
                {
                    Build();
                }
                else
                {
                    GameMgr.Get<IItemManager>().RemoveItem(backPackUI.GetCurrentItem().CurrentPosID, 1, new CargoData[] { data.backpack });
                    GameMgr.Get<ISkillManager>().UseSkill(this, backPackUI.GetCurrentItem().item.ItemFunc);
                }
                
            }
        }
    }
    public void Build()
    {        
        buildFunc.Invoke(this,backPackUI.GetCurrentItem().item.ItemFunc);
        GameMgr.Get<IItemManager>().RemoveItem(backPackUI.GetCurrentItem().CurrentPosID, 1, new CargoData[] { data.backpack });
    }

    public void OpenExitMenu()
    {
        if (InputManager.Instance.GetButtonDown(InputEnum.Quit)&&!isInteracting)
        {
            UIManager.Instance.Open("ExitMenuUI");
            isInteracting = true;
        }
    }
}
