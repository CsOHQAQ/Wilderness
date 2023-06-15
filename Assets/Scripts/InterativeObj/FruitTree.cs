using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class FruitTree : RenewableObj
{
    private bool isFull = false;//是否已经完成生长

    private float growPercentage = 0;//当前生长值

    private float maxGrowPercentage = 1;//最大生长值

    private float growHour = 24;//从0生长到maxGrowPercentage所需的时间

    private int fruitNum = 0;//这个树能提供多少原木


    public override void Init(MapBlock block, Object loadInstance = null)
    {
        base.Init(block, loadInstance);
        if (loadInstance != null && loadInstance is WoodTree)
        {
            //在此写读取存档的内容
            FruitTree load = (FruitTree)loadInstance;

        }
        Randomer rand = new Randomer();
        fruitNum = 0;
        if (rand.nextFloat() > 0.3f)//随机生成已经长好的树
        {
            growPercentage = maxGrowPercentage * (rand.nextFloat() * 0.25f + 0.75f);
            GrowFull();
        }
        else//未成熟的树
        {
            isFull = false;
            interactable = false;
            growPercentage = maxGrowPercentage * rand.nextFloat() * 0.25f;
            sprite.sprite = ResourceManager.Instance.Load<Sprite>("Texture/Plant/FruitTree_Half");
        }

    }
    public override void Interact(PlayerBase player)
    {
        base.Interact(player);
        //创建一个掉落物
        ItemPile item = new ItemPile();
        item.item = GameMgr.Get<IItemManager>().GetItemStatus(ItemManager.ItemsID["Fruit"]);
        item.CurrentPile = fruitNum;
        DropItem dropItem = ResourceManager.Instance.Instantiate("Prefabs/DropItem").GetComponent<DropItem>();
        dropItem.transform.position = player.transform.position + new Vector3(0, -1);
        dropItem.Init(item, player.data.backpack, 3.8f);
        Debug.Log($"#Interactive 与该这棵树交互，掉落{item.CurrentPile}个水果");

        //重置生长阶段
        growPercentage = 0;
        isFull = false;
        interactable = false;
        sprite.sprite= ResourceManager.Instance.Load<Sprite>("Texture/Plant/FruitTree_Half");
    }
    public override void Refresh(GameDateTime current)
    {
        if (lastVisitTime == current)
            return;
        growPercentage += ((current - lastVisitTime).TotalMinutes / 60.0f) * (1 / growHour);//更新生长阶段
        if (growPercentage > 0.75f && !isFull)
        {
            GrowFull();
        }
        if (growPercentage > maxGrowPercentage)
            growPercentage = maxGrowPercentage;

        base.Refresh(current);
    }

    /// <summary>
    /// 果树完成成长触发
    /// </summary>
    private void GrowFull()
    {
        Debug.Log($"#Map位于{transform.position}的树成熟了");
        isFull = true;
        interactable = true;
        fruitNum += new Randomer().nextInt( 2,5);
        sprite.sprite = ResourceManager.Instance.Load<Sprite>("Texture/Plant/FruitTree_Full");
    }

    public override void OnDestory()
    {
        base.OnDestory();
        Destroy(this.gameObject);
    }
}
