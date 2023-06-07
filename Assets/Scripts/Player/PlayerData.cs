using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class PlayerData
{
    protected string tableIndex = "player";
    [Tooltip("生命值")]
    private float maxHealth;
    private float health;
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }
    public float Health
    {
        get
        {
            return health;
        }
    }
    [Tooltip("饱腹值")]
    private float maxHunger;
    private float hunger;
    public float MaxHunger
    {
        get
        {
            return maxHunger;
        }
    }
    public float Hunger
    {
        get
        {
            return hunger;
        }
    }
    [Tooltip("温度,以100为满，0为最冷")]
    private float maxTemperature;
    private float temperature;
    public float MaxTemperature
    {
        get
        {
            return maxTemperature;
        }
    }
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

    public CargoData backpack;

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
        maxHealth = tab.GetFloat("HumanData",tableIndex,"health");
        maxHunger= tab.GetFloat("HumanData", tableIndex, "hunger");
        velocity = tab.GetFloat("HumanData", tableIndex, "velocity");
        maxTemperature = 100;
        Position = new Vector2(0,0);
        backpack = new CargoData();
        backpack.MaxBattery = 7;
    }
    public void ResetState()
    {
        health = maxHealth;
        hunger = maxHunger;
        temperature = maxTemperature;
    }

    //每次update刷新一次
    public void RefreshData(float environmentTemp)
    {
        float consumeHungerDay = MaxHunger / (Time.deltaTime*60*24);//分子是消耗的hunger，分母(去掉deltaTime的常数部分)是游戏中经过的分钟
        float consumeTemperatureHour = (environmentTemp-temperature) * 4 / (Time.deltaTime * 60);
         
        ChangeHunger(consumeHungerDay);
        ChangeTemperature(consumeTemperatureHour);
        if (Hunger > (3 * MaxHunger / 4)&&Temperature>(3*MaxTemperature))
        {
            ChangeHealth(MaxHealth / (Time.deltaTime * 8));
        }
        if (Hunger < MaxHunger / 3)
        {
            ChangeHealth(-MaxHealth / (Time.deltaTime * 10));
        }
        if (Temperature> (3 * MaxTemperature / 4) && Temperature > (3 * MaxTemperature))
        {
            ChangeHealth(MaxHealth / (Time.deltaTime * 8));
        }
        if (Temperature < MaxTemperature / 3)
        {
            ChangeHealth(-MaxHealth / (Time.deltaTime * 10));
        }
    }

    public void ChangeHunger(float number)
    {
        hunger += number;
        //很抽象的防止超越上下界的写法
        if (hunger < 0)
            hunger = 0;
        if (hunger > maxHunger)
            hunger = maxHunger;
    }
    public void ChangeHealth(float number)
    {
        health += number;
        //很抽象的防止超越上下界的写法
        if (health < 0)
            health = 0;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void ChangeTemperature(float number)
    {
        temperature += number;
        //很抽象的防止超越上下界的写法
        if (temperature < 0)
            temperature = 0;
        if (temperature > maxTemperature)
            temperature = maxTemperature;
    }
}
