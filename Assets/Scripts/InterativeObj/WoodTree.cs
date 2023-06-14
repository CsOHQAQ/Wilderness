using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class WoodTree : RenewableObj
{
    private bool isFull = false;
    private float growPercentage=0;
    private float maxGrowPercentage = 1;
    private float growHour = 24;//从0生长到maxGrowPercentage所需的时间
    private int woodNum =0;
    public override void Init(MapBlock block, Object loadInstance = null)
    {
        base.Init(block, loadInstance);
        if (loadInstance != null && loadInstance is WoodTree)
        {
            //在此写读取存档的内容
            WoodTree load = (WoodTree)loadInstance;

        }
        Randomer rand = new Randomer();
        woodNum = rand.nextInt(1, 2);
        if (rand.nextFloat() > 0.7f)//随机生成已经长好的树
        {
            growPercentage = maxGrowPercentage*(rand.nextFloat()*0.25f+0.75f);
            GrowFull();
        }
        else
        {
            growPercentage = maxGrowPercentage*rand.nextFloat()*0.25f;
            sprite.sprite = ResourceManager.Instance.Load<Sprite>("Texture/Plant/WoodTree_Half");
        }

    }
    public override void Interact(PlayerBase player)
    {
        base.Interact(player);
        
        for(int i = 1; i < woodNum; i++)
        {
            GameMgr.Get<IItemManager>().AddItem(ItemManager.ItemsID["Wood"],1, player.data.backpack);
        }
        OnDestory();
    }
    public override void Refresh(GameDateTime current)
    {
        if (lastVisitTime == current)
            return;
        growPercentage += ((current-lastVisitTime).TotalMinutes / 10) * (1 / growHour);
        if (growPercentage > 0.75f && !isFull)
        {
            GrowFull();
        }
        if (growPercentage > maxGrowPercentage)
            growPercentage = maxGrowPercentage;
        
        base.Refresh(current);
    }

    
    private void GrowFull()
    {
        isFull = true;
        woodNum+=4 * new Randomer().nextInt(0, 2);
        sprite.sprite= ResourceManager.Instance.Load<Sprite>("Texture/Plant/WoodTree_Full");
    }

    public override void OnDestory()
    {
        base.OnDestory();
        Destroy(this.gameObject);
    }
}
