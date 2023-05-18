using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using QxFramework.Core;
public class MapManager:MonoBehaviour
{
    private Tilemap backgroundTilemap;
    //用于上层地面的tilemap
    private Tilemap decorationTilemap_Ground;
    //用于记录地面上装饰（如花朵）的tilemap
    private Tilemap decorationTilemap_Object;

    //分别对应上述tilemap的tile，需要在场景中提前设置
    public Tile backgroundTile;
    public Tile decGroundTile;
    public Tile decObjTile;

    private PlayerBase player;
    //最远刷新区域中心离玩家多远
    private float refreshArea = 100f;
    //所有创建的区块表
    public Dictionary<Vector2Int, MapBlock> mapBlocks;
    //需要刷新的区块列表
    public List<MapBlock> refreshingMapBlocks;
    
    //开始进入时的初始化，还没有接入存档系统
    public void Init(PlayerBase playerBase)
    {
        player = playerBase;
        backgroundTilemap = transform.Find("Background").GetComponent<Tilemap>();
        decorationTilemap_Ground= transform.Find("Decoration_Ground").GetComponent<Tilemap>();
        decorationTilemap_Object = transform.Find("Decoration_Object").GetComponent<Tilemap>();

        mapBlocks = new Dictionary<Vector2Int, MapBlock>();
        //如果有存档就在这里写读取罢！

    }

}
