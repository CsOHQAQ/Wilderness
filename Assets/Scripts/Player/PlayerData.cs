using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class PlayerData
{
    protected string tableIndex = "player";
    [Tooltip("生命值")]
    private float health;
    public float Health
    {
        get
        {
            return health;
        }
    }
    [Tooltip("饱腹值")]
    private float hunger;
    public float Hunger
    {
        get
        {
            return hunger;
        }
    }
    [Tooltip("温度,以100为满，0为最冷")]
    private float temperature;
    public float Temperature 
    {
        get
        {
            return temperature;
        }
    }

    [Tooltip("移速")]
    private float velocity;
    public float Velocity
    {
        get
        {
            return velocity;
        }
    }
    
    //待完成buff栏
    //[Tooltip("buff")]
    //private 
    [Tooltip("位置,此为用于存读档的位置记录,所以修改它是不能修改玩家位置的")]
    public Vector2 Position;

    public void Init(PlayerData saveData=null)
    {
        //读取存档
        if (saveData != null)
        {
            //待完成项
            Debug.LogError("醒醒，你还没做这里");
            return;
        }

        TableAgent tab = new TableAgent();
        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/HumanData").text);
        health = tab.GetFloat("HumanData",tableIndex,"health");
        hunger= tab.GetFloat("HumanData", tableIndex, "hunger");
        velocity = tab.GetFloat("HumanData", tableIndex, "velocity");
        temperature = 100;
        Position = new Vector2(0,0);
    }
    public void AddHunger(float number)
    {
        hunger += number;
        //很抽象的防止超越上下界的写法
        if (hunger < 0)
            hunger = 0;
        if (hunger > 100)
            hunger = 100;
    }
    public void AddHealth(float number)
    {
        health += number;
        //很抽象的防止超越上下界的写法
        if (health < 0)
            health = 0;
        if (health > 100)
            health = 100;
    }

    public void AddTemp(float number)
    {
        temperature += number;
        //很抽象的防止超越上下界的写法
        if (temperature < 0)
            temperature = 0;
        if (temperature > 100)
            temperature = 100;
    }
}
