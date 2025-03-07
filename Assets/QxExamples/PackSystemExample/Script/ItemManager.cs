﻿using System.Collections.Generic;
using UnityEngine;

public class ItemManager : LogicModuleBase,IItemManager
{
    public Dictionary<int, Item> Items = new Dictionary<int, Item>();
    static public Dictionary<string, int> ItemsID = new Dictionary<string, int>();
    public override void Init()
    {
        base.Init();
        InitItemStatus();
        InitItemData();
    }
    private void InitItemStatus()
    {
        Items = new Dictionary<int, Item>();
        ItemsID = new Dictionary<string, int>();
        List<string> AllItemsStatus = QxFramework.Core.QXData.Instance.TableAgent.CollectKey1("Items");
        for(int i=0;i< AllItemsStatus.Count; i++)
        {
            Item item = new Item();
            item.ItemID = int.Parse(AllItemsStatus[i]);
            item.ItemPrice = QxFramework.Core.QXData.Instance.TableAgent.GetInt("Items", AllItemsStatus[i].ToString(), "Price");
            item.ItemImg = QxFramework.Core.QXData.Instance.TableAgent.GetString("Items", AllItemsStatus[i].ToString(), "Image");
            item.ItemName = QxFramework.Core.QXData.Instance.TableAgent.GetString("Items", AllItemsStatus[i].ToString(), "Name");
            item.ItemCodeName = QxFramework.Core.QXData.Instance.TableAgent.GetString("Items", AllItemsStatus[i].ToString(), "CodeName");
            item.ItemDescription = QxFramework.Core.QXData.Instance.TableAgent.GetString("Items", AllItemsStatus[i].ToString(), "Description");
            item.ItemType = (ItemType)System.Enum.Parse(typeof(ItemType),QxFramework.Core.QXData.Instance.TableAgent.GetString("Items", AllItemsStatus[i].ToString(), "Type"));
            item.ItemFunc= QxFramework.Core.QXData.Instance.TableAgent.GetString("Items", AllItemsStatus[i].ToString(), "Func");
            item.MaxPile = QxFramework.Core.QXData.Instance.TableAgent.GetInt("Items", AllItemsStatus[i].ToString(), "MaxPile");
            Items.Add(int.Parse(AllItemsStatus[i]), item);
            ItemsID.Add(item.ItemCodeName, item.ItemID);
        }
    }
    private void InitItemData()
    {
        RefeshBattery();
    }

    //初始化仓库
    //建议放在玩家背包或地图等模块内部初始化
    //数据也最好读表而不要写在代码里
    public void RefeshBattery()
    {
    }
    private Item CreateItem(int ID)
    {
        return Items[ID];
    }

    private int GetFirstEmptyPile(CargoData cargo)//得到第一个空位
    {
        List<int> PileTempID = new List<int>();
        for (int i = 0; i < cargo.itemPiles.Count; i++)
        {
            PileTempID.Add(cargo.itemPiles[i].CurrentPosID);
        }
        for(int id=0; id <= PileTempID.Count; id++)
        {
            if (!PileTempID.Contains(id))
            {
                return id;
            }
        }
        return -1;
    }

    public bool AddItem(int Id, int Count, CargoData cargo)
    {
        while (Count > 0)
        {
            Count--;
            bool MaxPile = true;
            for (int i = 0; i < cargo.itemPiles.Count; i++)
            {
                if (cargo.itemPiles[i].item.ItemID == Id && cargo.itemPiles[i].CurrentPile < cargo.itemPiles[i].item.MaxPile)
                {
                    cargo.itemPiles[i].CurrentPile += 1;
                    MaxPile = false;
                    break;
                }
            }
            if (!MaxPile)
            {
                continue;
            }
            if (cargo.itemPiles.Count < cargo.MaxBattery)
            {
                ItemPile itemPile = new ItemPile();
                itemPile.item = Items[Id];
                itemPile.CurrentPile = 1;
                itemPile.CurrentPosID = GetFirstEmptyPile(cargo);
                cargo.itemPiles.Add(itemPile);
            }
            else
            {
                return false;
            }
        }
        RefreshAllCargoUI();
        return true;
    }
    public bool RemoveItem(int PosId, int Count, CargoData[] cargo)
    {
        while (Count > 0)
        {
            Count--;
            for (int j = 0; j < cargo.Length; j++)
            {
                for (int i = 0; i < cargo[j].itemPiles.Count; i++)
                {
                    if (cargo[j].itemPiles[i].CurrentPosID == PosId && cargo[j].itemPiles[i].CurrentPile > 0)
                    {
                        cargo[j].itemPiles[i].CurrentPile -= 1;
                        if (cargo[j].itemPiles[i].CurrentPile <= 0)
                        {
                            cargo[j].itemPiles.RemoveAt(i);
                        }
                        break;
                    }
                }
            }
        }
        RefreshAllCargoUI();
        return true;
    }
    public bool RemoveItemByID(int ItemID, int Count, CargoData[] cargo)
    {
        while (Count > 0)
        {
            Count--;
            for (int j = 0; j < cargo.Length; j++)
            {
                for (int i = 0; i < cargo[j].itemPiles.Count; i++)
                {
                    if (cargo[j].itemPiles[i].item.ItemID == ItemID && cargo[j].itemPiles[i].CurrentPile > 0)
                    {
                        cargo[j].itemPiles[i].CurrentPile -= 1;
                        if (cargo[j].itemPiles[i].CurrentPile <= 0)
                        {
                            cargo[j].itemPiles.RemoveAt(i);
                        }
                        break;
                    }
                }
            }
        }
        RefreshAllCargoUI();
        return true;
    }
    public bool CheckItemEnough(int ItemID, int Count, CargoData[] cargo)
    {
        int CountTemp = 0;
        for (int j = 0; j < cargo.Length; j++)
        {
            for (int i = 0; i < cargo[j].itemPiles.Count; i++)
            {
                if (cargo[j].itemPiles[i].item.ItemID == ItemID)
                {
                    CountTemp += cargo[j].itemPiles[i].CurrentPile;
                }
            }
        }
        return CountTemp >= Count;
    }
    public int GetItemCount(int ItemID, CargoData[] cargo)
    {
        int CountTemp = 0;
        for (int j = 0; j < cargo.Length; j++)
        {
            for (int i = 0; i < cargo[j].itemPiles.Count; i++)
            {
                if (cargo[j].itemPiles[i].item.ItemID == ItemID)
                {
                    CountTemp += cargo[j].itemPiles[i].CurrentPile;
                }
            }
        }
        return CountTemp;
    }
    public bool CheckEmptyPile(CargoData[] cargo, out CargoData emptyCargo)
    {
        emptyCargo = null;
        for (int j = 0; j < cargo.Length; j++)
        {
            if (cargo[j].itemPiles.Count < cargo[j].MaxBattery)
            {
                emptyCargo = cargo[j];
                return true;
            }
        }
        return false;
    }

    public bool RemoveItemToCargo(int PosID, int Count, CargoData cargo, CargoData cargoTo)
    {
        while (Count > 0)
        {
            Count--;
            int Id = 0;
            for (int i = 0; i < cargo.itemPiles.Count; i++)
            {
                if (cargo.itemPiles[i].CurrentPosID == PosID)
                {
                    Id = cargo.itemPiles[i].item.ItemID;
                }
            }

            if (AddItem(Id, 1, cargoTo))
            {
                RemoveItem(PosID, 1, new CargoData[] { cargo });
            }
        }

        return true;
    }
    public bool TakeAllItemFrom(CargoData[] cargoAll)//第0个是来源
    {
        for (int j = 1; j < cargoAll.Length; j++)
        {
            while (cargoAll[0].itemPiles.Count > 0)
            {
                for (int i = 0; i < cargoAll[0].itemPiles.Count; i++)
                {
                    if(cargoAll[0].itemPiles[i].CurrentPile == 0)
                    {
                        cargoAll[0].itemPiles.RemoveAt(i);
                        break;
                    }
                    RemoveItemToCargo(cargoAll[0].itemPiles[i].CurrentPosID, cargoAll[0].itemPiles[i].CurrentPile, cargoAll[0], cargoAll[j]);
                }
                if(cargoAll[j].itemPiles.Count>= cargoAll[j].MaxBattery)
                {
                    break;
                }
            }
        }
        return true;
    }
    public bool putAllItemTo(CargoData[] cargoAll)//第0个是去向
    {
        for (int j = 1; j < cargoAll.Length; j++)
        {
            while (cargoAll[j].itemPiles.Count > 0)
            {
                for (int i = 0; i < cargoAll[j].itemPiles.Count; i++)
                {
                    if (cargoAll[j].itemPiles[i].CurrentPile == 0)
                    {
                        cargoAll[j].itemPiles.RemoveAt(i);
                        break;
                    }
                    RemoveItemToCargo(cargoAll[j].itemPiles[i].CurrentPosID, cargoAll[j].itemPiles[i].CurrentPile, cargoAll[j], cargoAll[0]);
                }
                if (cargoAll[0].itemPiles.Count >= cargoAll[0].MaxBattery)
                {
                    break;
                }
            }
        }
        RefreshAllCargoUI();
        return true;
    }
    public void ResolvePackage(CargoData cargo)
    {
        CargoData TempCargo = new CargoData();
        TempCargo.MaxBattery = 99;
        TakeAllItemFrom(new CargoData[] { cargo, TempCargo });
        putAllItemTo(new CargoData[] { cargo, TempCargo });
    }



    public void SwitchPile(CargoData cargoFrom, ItemPile fromPile, CargoData cargoTo, ItemPile toPile)
    {
        if (fromPile.item.ItemID == toPile.item.ItemID)
        {
            if(toPile.CurrentPile < toPile.item.MaxPile)
            {
                int TempNum = toPile.item.MaxPile - toPile.CurrentPile;
                if (TempNum >= fromPile.CurrentPile)
                {
                    for (int i = 0; i < cargoTo.itemPiles.Count; i++)
                    {
                        if (cargoTo.itemPiles[i].CurrentPosID == toPile.CurrentPosID)
                        {
                            cargoTo.itemPiles[i].CurrentPile += fromPile.CurrentPile;
                        }
                    }
                    for (int i = 0; i < cargoFrom.itemPiles.Count; i++)
                    {
                        if (cargoFrom.itemPiles[i].CurrentPosID == fromPile.CurrentPosID)
                        {
                            cargoFrom.itemPiles.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cargoTo.itemPiles.Count; i++)
                    {
                        if (cargoTo.itemPiles[i].CurrentPosID == toPile.CurrentPosID)
                        {
                            cargoTo.itemPiles[i].CurrentPile += TempNum;
                        }
                    }
                    for (int i = 0; i < cargoFrom.itemPiles.Count; i++)
                    {
                        if (cargoFrom.itemPiles[i].CurrentPosID == fromPile.CurrentPosID)
                        {
                            cargoFrom.itemPiles[i].CurrentPile -= TempNum;
                        }
                    }
                }
            }
            return;
        }

        int ToPileID = toPile.CurrentPosID;
        int ToPileCount = toPile.CurrentPile;
        Item itmTo = toPile.item;
        int FromPileID = fromPile.CurrentPosID;
        int FromPileCount = fromPile.CurrentPile;
        Item itmFrom = fromPile.item;
        for (int i = 0; i < cargoFrom.itemPiles.Count; i++)
        {
            if (cargoFrom.itemPiles[i].CurrentPosID == FromPileID)
            {
                cargoFrom.itemPiles.RemoveAt(i);
            }
        }
        cargoFrom.itemPiles.Add(new ItemPile()
        {
            CurrentPosID = FromPileID,
            CurrentPile = ToPileCount,
            item = itmTo
        });
        for (int i = 0; i < cargoTo.itemPiles.Count; i++)
        {
            if (cargoTo.itemPiles[i].CurrentPosID == ToPileID)
            {
                cargoTo.itemPiles.RemoveAt(i);
            }
        }
        cargoTo.itemPiles.Add(new ItemPile()
        {
            CurrentPosID = ToPileID,
            CurrentPile = FromPileCount,
            item = itmFrom
        });
    }
    public void SwitchPile(CargoData cargoFrom, ItemPile fromPile, CargoData cargoTo, int toPosID)
    {
        for (int i = 0; i < cargoFrom.itemPiles.Count; i++)
        {
            if (cargoFrom.itemPiles[i].CurrentPosID == fromPile.CurrentPosID)
            {
                cargoFrom.itemPiles.RemoveAt(i);
            }
        }
        fromPile.CurrentPosID = toPosID;
        cargoTo.itemPiles.Add(fromPile);
    }
    public Item GetItemStatus(int Id)
    {
        try
        {
            return Items[Id];
        }
        catch
        {
            Debug.LogError(Id);
        }
        return Items[Id];
    }
    public void RefreshAllCargoUI()
    {
        PackBase[] packUIs= GameObject.FindObjectsOfType<PackBase>();
        for(int i =0;i< packUIs.Length; i++)
        {
            packUIs[i].RefreshUI();
        }
    }
}
public class CargoData 
{
    public int MaxBattery = 10;
    public bool Locked = false;
    public bool IsShop = false;
    public List<ItemPile> itemPiles = new List<ItemPile>();
}
public class ItemPile
{
    public Item item = new Item();
    public int CurrentPosID = 0;
    public int CurrentPile = 0;
}
public class Item
{
    public int ItemID = 1001;
    public int ItemPrice = 1001;
    public string ItemImg = "";
    public string ItemCodeName = "";
    public string ItemName = "";
    public string ItemDescription = "";
    public string ItemFunc = "";
    public string ItemSound = "";
    public ItemType ItemType = ItemType.Props;
    public int MaxPile = 1;
}
public enum ItemType
{
    Props,
    Material,
    Building,
}