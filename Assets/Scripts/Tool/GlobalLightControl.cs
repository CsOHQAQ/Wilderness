using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using QxFramework.Core;

public class GlobalLightControl : MonoBehaviour
{
    public float lowestNaturalLightIntensity=0.05f;
    public float highestNaturalLightIntensity = 1f;
    public Light2D globalLight;
    public void Init()
    {
        globalLight = GetComponent<Light2D>();
    }
   
    void Update()
    {
        GameDateTime curTime = GameMgr.Get<IGameTimeManager>().GetNow();
        curTime = curTime - new GameDateTime(curTime.Days,0,0);//忽略日期影响，只考虑小时和分钟
        //亮度调节，考虑从5点开始天亮，并于10点达到最大亮度；从18点开始天黑，并于晚上11点达到最小亮度。
        float intense= Mathf.Cos(((float)curTime.TotalMinutes * Mathf.PI * 2) /(24.0f*60.0f)+Mathf.PI)/1.5f+0.5f;
        intense = Mathf.Max(intense, lowestNaturalLightIntensity);
        intense = Mathf.Min(intense, highestNaturalLightIntensity);
        globalLight.intensity = intense;
        
    }
}
