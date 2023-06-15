using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using System;
using System.Reflection;

public class SkillManager : LogicModuleBase,ISkillManager
{
    public SkillData skillData;
    public override void Init()
    {
        base.Init();
        if (!RegisterData(out skillData))
        {
            skillData.CurrentSkill = 0;
        }
    }

    public void UseSkill(PlayerBase human,string Func)
    {
        if (Func == "")
        {
            return;
        }
        Type t = typeof(SkillManager);//括号中的为所要使用的函数所在的类的类名。

        if (Func.Contains(":"))
        {
            MethodInfo mt = t.GetMethod(Func.Split(':')[0]);
            if (mt == null)
            {
                Debug.Log("No Function: " + Func);
            }
            mt.Invoke(null, new object[] {human, Func.Split(':')[1] });
        }
        else
        {
            MethodInfo mt = t.GetMethod(Func);
            if (mt == null)
            {
                Debug.Log("No Function: " + Func);
            }
            mt.Invoke(null, new object[] { human, "" });
        }
    }

    #region 技能函数

    public static void Food(PlayerBase human,string Parm)
    {
        float addHunger = float.Parse(Parm);
        human.data.ChangeHunger(addHunger);
    }
    #endregion
}
public class SkillData : GameDataBase
{
    public int CurrentSkill;
}
