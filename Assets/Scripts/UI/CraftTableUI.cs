using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using QxFramework.Core;

public class CraftTableUI : UIBase
{
    private PlayerBase player;
    private Image itemImg;
    private Text itemName;
    private Text itemDes;
    private Button confirmBtn;
    private Button exitBtn;
    private TableAgent tab;
    private string curSelectCraft = "";
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        #region 获取组件进行初始化
        player = (PlayerBase)args;
        itemImg = Get<Image>("ItemImg");
        itemName = Get<Text>("ItemName");
        itemDes = Get<Text>("ItemDescription");
        confirmBtn = Get<Button>("ConfirmBtn");
        exitBtn = Get<Button>("ExitBtn");
        tab = QXData.Instance.TableAgent;
        //初始化组件的部分值
        itemName.text = "合成表";
        itemDes.text = "";
        itemImg.sprite = ResourceManager.Instance.Load<Sprite>("Texture/UI/BaseFrame");
        confirmBtn.onClick.SetListener(() =>{
            Craft();
        });
        confirmBtn.interactable = false;
        exitBtn.onClick.AddListener(() => {
            player.isInteracting = false;
            UIManager.Instance.Close(this);
        });
        #endregion

        #region 清除上一次显示的合成列表
        int childCount = Get<Transform>("ScrollRect").childCount;
        for (int i = 0; i < childCount; i++)
        {
            UIBase child = Get<Transform>("ScrollRect").GetChild(0).GetComponent<UIBase>();
            if (child == null)//如果存在不是UI的GameObj出现就删除
            {
                GameObject.Destroy(Get<Transform>("ScrollRect").GetChild(0).gameObject);
            }
            else//否则使用UIManager的正常关闭
            {
                UIManager.Instance.Close(child);
            }
        }

        childCount = Get<Transform>("NeedItemList").childCount;
        for (int i = 0; i < childCount; i++)
        {
            UIBase child = Get<Transform>("NeedItemList").GetChild(0).GetComponent<UIBase>();
            if (child == null)//如果存在不是UI的GameObj出现就删除
            {
                GameObject.Destroy(Get<Transform>("NeedItemList").GetChild(0).gameObject);
            }
            else//否则使用UIManager的正常关闭
            {
                UIManager.Instance.Close(child);
            }
        }

        #endregion

        #region 从CraftTable中获取所有的合成表
        List<string> allCraftTable = tab.CollectKey1("CraftTable");
        foreach(var tableName in allCraftTable)
        { 
            UIBase needItem = UIManager.Instance.Open("NeedItemUI");
            needItem.transform.SetParent(Get<Transform>("ScrollRect"));
            needItem.Get<Text>("Text").text = "";
            needItem.Get<Image>("ItemImg").sprite = ResourceManager.Instance.Load<Sprite>("Texture/Property/"+tab.GetString("CraftTable",tableName,"Image"));
            needItem.GetComponent<Button>().onClick.AddListener(()=>{SetCurCraft(tableName);});
        }
        #endregion
    }

    protected override void OnClose()
    {
        base.OnClose();
        player.isInteracting = false;
    }
    private void Update()
    {
        if(InputManager.Instance.GetButtonDown(InputEnum.Quit)|| InputManager.Instance.GetButtonDown(InputEnum.Craft))//添加两个关闭的快捷键
        {
            UIManager.Instance.Close(this);
        }
    }

    private Dictionary<string,int> GetNeedItemList(string str)
    {
        Dictionary<string, int> dic=new Dictionary<string, int>();
        foreach(string item in str.Split('|'))
        {
            dic.Add(item.Split(':')[0], int.Parse(item.Split(':')[1]));
        }
        return dic;
    }

    /// <summary>
    /// 将右侧面板显示为当前选中的合成表
    /// </summary>
    private void SetCurCraft(string tableName)
    {
        #region 清除上一次显示的需求列表
        int childCount = Get<Transform>("NeedItemList").childCount;
        for (int i = 0; i < childCount; i++) 
        {
            UIBase child = Get<Transform>("NeedItemList").GetChild(0).GetComponent<UIBase>();
            if (child == null)//如果存在不是UI的GameObj出现就删除
            {
                GameObject.Destroy(Get<Transform>("NeedItemList").GetChild(0).gameObject);
            }
            else//否则使用UIManager的正常关闭
            {
                UIManager.Instance.Close(child);
            }
        }
        #endregion

        #region 设置右侧面板
        curSelectCraft = tableName;
        itemImg.sprite = ResourceManager.Instance.Load<Sprite>("Texture/Property/"+tab.GetString("CraftTable",curSelectCraft,"Image"));
        itemName.text = tab.GetString("CraftTable", curSelectCraft, "Name");
        itemDes.text = tab.GetString("CraftTable", curSelectCraft, "Description");
        #endregion

        bool canCraft = true;

        #region 将合成所需材料依次显示到NeedItemList中
        Dictionary<string, int> needItemDic = GetNeedItemList(tab.GetString("CraftTable",curSelectCraft,"NeedItemList"));
        foreach (string name in needItemDic.Keys )//按照每种需求的item创建UI
        {
            //初始化
            UIBase needItem = UIManager.Instance.Open("NeedItemUI");
            needItem.transform.SetParent(Get<Transform>("NeedItemList"));            
            needItem.Get<Image>("ItemImg").sprite = ResourceManager.Instance.Load<Sprite>("Texture/Property/" 
                +GameMgr.Get<IItemManager>().GetItemStatus(ItemManager.ItemsID[name]).ItemImg);
            needItem.GetComponent<Button>().onClick.RemoveAllListeners();

            int playerHave = GameMgr.Get<IItemManager>().GetItemCount(ItemManager.ItemsID[name], new CargoData[] { player.data.backpack }), totalNeed = needItemDic[name];
            needItem.Get<Text>("Text").text =playerHave .ToString()//获取玩家背包中有几个该item
                + "/" +totalNeed .ToString();//总共需要几个该item

            if (playerHave < totalNeed)
            {
                needItem.Get<Text>("Text").color = Color.red;
                canCraft = false;//顺手检测是否能够合成该item
            }//如果玩家持有该种道具的数量不足，则字体为红色，否则为白色

            else
            {
                needItem.Get<Text>("Text").color = Color.white;
            }
            

         confirmBtn.interactable = canCraft;
        }
        #endregion
    }

    /// <summary>
    /// 按照当前选中的合成表合成一个物品
    /// </summary>
    private void Craft()
    {
        if (!tab.CollectKey1("CraftTable").Contains(curSelectCraft))
        {
            Debug.LogError($"#Craft合成表中不存在{curSelectCraft}");
            return;
        }

        Dictionary<string, int> needItemDic = GetNeedItemList(tab.GetString("CraftTable", curSelectCraft, "NeedItemList"));
        foreach (string name in needItemDic.Keys)
        {
            int itemID = ItemManager.ItemsID[name];
            GameMgr.Get<IItemManager>().RemoveItemByID(itemID,needItemDic[name],new CargoData[] { player.data.backpack});
        }//依次从玩家backpack中移除合成的各种物品

        if (!GameMgr.Get<IItemManager>().AddItem(ItemManager.ItemsID[tab.GetString("CraftTable", curSelectCraft, "TargetItem")], 1, player.data.backpack))//如果不能成功的向玩家背包添加物品，就在地面生成一个dropItem
        {
            ItemPile item = new ItemPile();
            item.item = GameMgr.Get<IItemManager>().GetItemStatus(ItemManager.ItemsID[tab.GetString("CraftTable", curSelectCraft, "TargetItem")]);
            item.CurrentPile = 1;
            DropItem dropItem = ResourceManager.Instance.Instantiate("Prefabs/DropItem").GetComponent<DropItem>();
            dropItem.transform.position = player.transform.position + new Vector3(0, -1);
            dropItem.Init(item, player.data.backpack);
        }
        SetCurCraft(curSelectCraft);//通过重新选择刷新一遍ui
    }

}
