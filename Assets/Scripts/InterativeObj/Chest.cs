using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class Chest : Building
{
    public CargoData cargo;
    public override void Init(MapBlock block, Object loadInstance = null)
    {
        base.Init(block, loadInstance);
        cargo = new CargoData();
        cargo.MaxBattery = 10;
        interactable = false;
    }
    public override void Interact(PlayerBase player)
    {
        base.Interact(player);
        UIManager.Instance.Open("ChestCargoUI",2,"",new CargoData[] { cargo,player.data.backpack}).GetComponent<ChestCargoUI>().SetPlayer(player);
        player.isInteracting = true;
    }

}
