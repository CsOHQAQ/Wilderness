using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using QxFramework.Core;
public class MapManager:MonoBehaviour
{
    private bool isInited = false;

    private PlayerBase player;
    //最远刷新区域中心离玩家多远
    private float refreshArea = 100f;
    //区块半径(暂时不建议修改，因为预设挺难调的)
    private int blockSize = 20;
    //所有创建的区块表，索引为区块左下角的坐标
    public Dictionary<Vector2Int, MapBlock> mapBlocks=new Dictionary<Vector2Int, MapBlock>();
    //需要刷新的区块列表
    public List<MapBlock> refreshingMapBlocks=new List<MapBlock>();
    private List<MapBlock> removeRecord = new List<MapBlock>();
    
    //开始进入时的初始化，还没有接入存档系统
    public void Init(PlayerBase playerBase)
    {
        player = playerBase;
        player.SetInteractFunc(Interact);
        //如果有存档就在这里写读取罢！
        for(int i = -4; i <= 4; i++)
        {
            for(int j = -4; j <= 4; j++)
            {
                CreateMapBlock(new Vector2Int(i, j) * 2 * blockSize, true);
            }
        }
        Debug.Log("#Map完成地图块初始生成");
        isInited = true;//完成初始化，可以开始Update
    }

    public void Update()
    {
        if (!isInited)//防止在未初始化时运行
            return;

        //检查玩家是否会看见未加载区块
        int viewAreaHeight = (int)Camera.main.orthographicSize / (blockSize * 2);
        int viewAreaWidth = (int)(Camera.main.orthographicSize*Camera.main.aspect / (blockSize * 2));
        for(int i = -viewAreaHeight - 2; i <= viewAreaHeight +2; i++)
        {
            for(int j = -viewAreaWidth - 2; j <= viewAreaWidth + 2; j++)
            {
                Vector2Int blockPos = new Vector2Int(playerPos2MapBlockPos().x+j*2*blockSize,playerPos2MapBlockPos().y+i*2*blockSize );
                if (!mapBlocks.ContainsKey(blockPos))
                {
                    Debug.Log($"#Map 在{blockPos}位置创建新的区块");
                    CreateMapBlock(blockPos);
                }

                if (!refreshingMapBlocks.Contains(mapBlocks[blockPos]))
                {
                    mapBlocks[blockPos].EnterRefresh(GameMgr.Get<IGameTimeManager>().GetNow());
                    refreshingMapBlocks.Add(mapBlocks[blockPos]);
                }
            }
        }

        foreach(var block in refreshingMapBlocks)
        {
            if (Vector2.Distance(block.leftDownPosition, playerMapPos()) > refreshArea)
            {
                block.ExitRefresh();
                removeRecord.Add(block);
            }
            else
            {
                block.Refresh(GameMgr.Get<IGameTimeManager>().GetNow());
            }
        }

        foreach(var reblock in removeRecord)
        {
            refreshingMapBlocks.Remove(reblock);
        }
        removeRecord.Clear();

        mapBlocks[playerPos2MapBlockPos()].ChectInteractiveItem(player);
        SetPlayerTemperature();

        ShowBlockArea();//在Scene中显示区块的大小
    }

    /// <summary>
    /// 创建新的区块
    /// </summary>
    /// <param name="mapBlockPos"></param>
    /// <param name="instantCreate"></param>
    private void CreateMapBlock(Vector2Int mapBlockPos, bool instantCreate = false)
    {
        GameObject block = ResourceManager.Instance.Instantiate("Prefabs/Grid/BackgroundGrid");
        block.name = ($"block_({mapBlockPos.x},{mapBlockPos.y})");
        block.transform.position = new Vector3(mapBlockPos.x, mapBlockPos.y);
        block.transform.parent = this.transform;
        block.GetComponent<MapBlock>().leftDownPosition = mapBlockPos;
        mapBlocks.Add(mapBlockPos, block.GetComponent<MapBlock>());
        block.GetComponent<MapBlock>().Init(instantCreate);
    }


    private void Interact(PlayerBase player)
    {
        MapBlock curBlock = mapBlocks[playerPos2MapBlockPos()];
        curBlock.Interact(player);
    }

    /// <summary>
    /// 用于在debug时展现一个区块的范围
    /// </summary>
    private void ShowBlockArea()
    {
        foreach (var block in mapBlocks.Values)
        {
            Vector3 leftDown =new Vector3(block.leftDownPosition.x,block.leftDownPosition.y),
                leftUp=new Vector3(block.leftDownPosition.x, block.leftDownPosition.y+blockSize*2),
                rightDown=new Vector3(block.leftDownPosition.x+blockSize*2,block.leftDownPosition.y),
                rightUp=new Vector3(block.leftDownPosition.x + blockSize * 2, block.leftDownPosition.y+blockSize*2);
            Debug.DrawLine(leftDown,leftUp,Color.red);
            Debug.DrawLine(leftDown, rightDown, Color.red);
            Debug.DrawLine(rightUp, leftUp, Color.red);
            Debug.DrawLine(rightUp, rightDown, Color.red);
        }
        foreach(var block in refreshingMapBlocks)
        {
            Vector3 leftDown = new Vector3(block.leftDownPosition.x, block.leftDownPosition.y),
                leftUp = new Vector3(block.leftDownPosition.x, block.leftDownPosition.y + blockSize * 2),
                rightDown = new Vector3(block.leftDownPosition.x + blockSize * 2, block.leftDownPosition.y),
                rightUp = new Vector3(block.leftDownPosition.x + blockSize * 2, block.leftDownPosition.y + blockSize * 2);
            Debug.DrawLine(leftDown, leftUp, Color.blue);
            Debug.DrawLine(leftDown, rightDown, Color.blue);
            Debug.DrawLine(rightUp, leftUp, Color.blue);
            Debug.DrawLine(rightUp, rightDown, Color.blue);
        }
    }
    
    private void SetPlayerTemperature()
    {
        Vector2Int playerPos = playerMapPos();
        player.environmentTemp = mapBlocks[playerPos2MapBlockPos()].environmentTemperature[playerPos.y,playerPos.x];
    }

    private Vector2Int playerMapPos()
    { 
        int x=0,y=0;
        x = (int)((Mathf.Abs(player.transform.position.x) + 0.5f) * Mathf.Sign(player.transform.position.x));
        y = (int)((Mathf.Abs(player.transform.position.y) + 0.5f) * Mathf.Sign(player.transform.position.y));
        return new Vector2Int(x, y);
    }

    private Vector2Int playerPos2MapBlockPos()
    {
        Vector2Int playerPos = playerMapPos();
        int x = 0, y = 0;
        x+= playerPos.x / (2 * blockSize);
        x -= playerPos.x < 0 ? 1 : 0;
        y+= playerPos.y / (2 * blockSize);
        y -= playerPos.y < 0 ? 1 : 0;
        return new Vector2Int(x*2*blockSize,y*2*blockSize);
    }
}
