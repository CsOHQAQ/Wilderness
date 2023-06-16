using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using QxFramework.Core;

public class ChestCargoUI : PackBase
{
    private PlayerBase player;
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
    }
    public void SetPlayer(PlayerBase p)
    {
        player = p;
    }
    protected override void OnClose()
    {
        base.OnClose();
        player.isInteracting = false;
    }
}
