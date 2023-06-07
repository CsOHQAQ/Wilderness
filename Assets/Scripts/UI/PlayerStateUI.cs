using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using QxFramework.Core;
public class PlayerStateUI : UIBase
{
    private PlayerBase player;
    private Text timeText;
    private Image healthBar;
    private Image coldBar;
    private Image hungerBar;

    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        if(! (args is PlayerBase))
        {
            Debug.LogError("传入的参数不是PlayerBase");
            UIManager.Instance.Close(this);
            return;
        }
        player = (PlayerBase)args;
        timeText= Get<Text>("TimeText");
        healthBar = Get<Image>("HealthBar");
        coldBar= Get<Image>("ColdBar");
        hungerBar= Get<Image>("HungerBar");
    }
    private void Update()
    {
        timeText.text = GameMgr.Get<IGameTimeManager>().GetNow().ToMomentString();
        healthBar.fillAmount = player.data.Health / player.data.MaxHealth;
        coldBar.fillAmount = player.data.Temperature / player.data.MaxTemperature;
        hungerBar.fillAmount=player.data.Hunger / player.data.MaxHunger;
    }
}
