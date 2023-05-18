using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class MapBlock 
{
    public Vector2Int Position;
    public int ID;
    public Dictionary<Vector2Int,SingleMapBlock> singleBlocks;
    public GameDateTime lastVisitTime;
    private int size = 10;//这个是一半的长
    
    public void FirstCreate()
    {

    }

    public void Init()
    {

    }

    public void EnterRefresh()
    {

    }

    public void ExitRefresh()
    {

    }

}

public class SingleMapBlock
{
    public Sprite sprite;
}