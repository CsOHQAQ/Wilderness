using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
public class BuffManager : MonoBehaviour
{
    public  List<Buff> BuffList;
    private List<Buff> DisposeList;
    public DataChanger dataChanger
    {
        get
        {
            DataChanger data = new DataChanger();
            foreach (var buff in BuffList)
            {
                
                data += buff.data;

            }
            return data;

        }
    }
    public BuffSystemEntity entity;

    public void Init()
    {
        BuffList = new List<Buff>();
        entity = new BuffSystemEntity();
        DisposeList = new List<Buff>();
    }
    protected virtual void FixedUpdate()
    {
        
    }
    protected virtual void Update()
    {


        foreach (var buff in BuffList)
        {
            
            buff.Refresh();
            if (buff.LastingTime <= 0)
            {
                DisposeList.Add(buff);
            }
        }
        foreach (var b in DisposeList)
            RemoveBuff(b);
        DisposeList.Clear();
    }

    //这个需要填写添加buff的种类与所加buff的积累值
    public void AddBuff(Buff buff,float AddTime,float AddCount=0)
    {
        foreach(var b in BuffList)
        {
            if (b.GetType() ==buff.GetType() )//此处默认相同种类的buff会自动叠加持续时间和效果
            {
                b.LastingTime += AddTime;
                b.Count += AddCount;
                Debug.Log("已增加" + b + "的持续时间至" + b.LastingTime);

                if (b.Count > b.MaxCount)
                    b.ActivateWhenFull();
                return;
            }
        }

        buff.SetManager(this);
        buff.Count = AddCount;
        buff.LastingTime = AddTime;
        buff.Init();
        BuffList.Add(buff);
        Debug.Log("已添加" + buff);
    }   

    //移除buff用,不要直接remove会无法消除影响
    public void RemoveBuff(Buff removedBuff)
    {
        for(int i = 0; i < BuffList.Count; i++)
        {
            var b = BuffList[i];

            if (b.GetType() == removedBuff.GetType())
            {
                b.BeforeRemove();
                BuffList.Remove(b);
                return;
            }
        }
    }

    public void RemoveAllBuff()
    {
        foreach (var buff in BuffList)
        {
            DisposeList.Add(buff);
        }

        foreach (var b in DisposeList)
            RemoveBuff(b);
        DisposeList.Clear();
    } 
    public List <Buff> CurrentBuff()
    {
        return BuffList;
    }

}
/// <summary>
/// Buff产生的数据修改。
/// </summary>
public class DataChanger
{

    public AddIndex MoveSpeedMul;    //示例用参数，影响entity的移速
    public DataChanger()
    {
        MoveSpeedMul = new AddIndex(0);
    }
    /// <summary>
    /// 这一段是处理dataChanger之间叠加的效果。请注意，为了减少重复的代码计算，本段使用了反射。
    /// </summary>
    public static DataChanger operator+(DataChanger d1,DataChanger d2)
    {
        DataChanger data = new DataChanger();

        //如果不使用反射，则需要自行处理数据叠加的方式
        #region 反射处理数据叠加部分
        FieldInfo[] fields = d1.GetType().GetFields();//只获取公开的字段
        foreach(var field in fields)
        {
            IndexOperator i1 = (IndexOperator)field.GetValue(d1),
                i2 = (IndexOperator)field.GetValue(d2);
            field.SetValue(data, i1.Add(i1, i2));
        }
        #endregion
        return data;
    }
    public static DataChanger operator -(DataChanger d1, DataChanger d2)
    {
        DataChanger data = new DataChanger();
        //如果不使用反射，则需要自行处理数据叠加的方式
        #region 反射处理数据叠加部分
        FieldInfo[] fields = d1.GetType().GetFields();//只获取公开的字段
        foreach (var field in fields)
        {
            IndexOperator i1 = (IndexOperator)field.GetValue(d1),
                i2 = (IndexOperator)field.GetValue(d2);
            field.SetValue(data, i1.Minus(i1, i2));
        }
        #endregion
        return data;
    }

}


#region 系数计算方式
public abstract class IndexOperator
{
    public float _index;
    public abstract IndexOperator Add(IndexOperator i1, IndexOperator i2);
    public abstract IndexOperator Minus(IndexOperator i1, IndexOperator i2);
}
/// <summary>
/// 加算，系数之间直接相加
/// </summary>
/// <example>两个加20%的buff叠加后为40%</example>
public class AddIndex:IndexOperator
{
    public AddIndex(float index)
    {
        _index = index;
    }
    public override IndexOperator Add(IndexOperator i1, IndexOperator i2)
    {
        return new AddIndex(i1._index + i2._index);
    }
    public override IndexOperator Minus(IndexOperator i1, IndexOperator i2)
    {
        return new AddIndex(i1._index - i2._index);
    }
}
/// <summary>
/// 乘算，系数之间直接相乘
/// </summary>
/// <example>两个加20%的buff叠加后为44%</example>
public class MulIndex:IndexOperator
{
    public MulIndex(float index)
    {
        _index = index;
    }
    public override IndexOperator Add(IndexOperator i1, IndexOperator i2)
    {
        return new MulIndex(i1._index * i2._index);
    }
    public override IndexOperator Minus(IndexOperator i1, IndexOperator i2)
    {
        return new MulIndex(i1._index / i2._index);
    }
}

#endregion