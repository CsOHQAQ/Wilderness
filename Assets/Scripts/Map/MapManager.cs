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

        //在初始化大地图预先在玩家周围生成如下这么多个地块，防止后续卡顿
        for(int i = -3; i <= 3; i++)
        {
            for(int j = -3; j <= 3; j++)
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

        #region 检查玩家是否会看见未加载区块
        int viewAreaHeight = (int)Camera.main.orthographicSize / (blockSize * 2);//玩家视野高度（从摄像机坐标系转到为若干个区块）
        int viewAreaWidth = (int)(Camera.main.orthographicSize*Camera.main.aspect / (blockSize * 2));//玩家视野宽度（从摄像机坐标系转到为若干个区块）
        for (int i = -viewAreaHeight - 2; i <= viewAreaHeight +2; i++)
        {
            for(int j = -viewAreaWidth - 2; j <= viewAreaWidth + 2; j++)
            {
                Vector2Int blockPos = new Vector2Int(playerPos2MapBlockPos().x+j*2*blockSize,playerPos2MapBlockPos().y+i*2*blockSize );

                if (!mapBlocks.ContainsKey(blockPos))//该区块未被创建
                {
                    Debug.Log($"#Map 在{blockPos}位置创建新的区块");
                    CreateMapBlock(blockPos);
                }

                if (!refreshingMapBlocks.Contains(mapBlocks[blockPos]))//将该区块加入刷新队列
                {
                    mapBlocks[blockPos].EnterRefresh(GameMgr.Get<IGameTimeManager>().GetNow());
                    refreshingMapBlocks.Add(mapBlocks[blockPos]);
                }
            }
        }
        #endregion

        #region 检查刷新队列中是否存在远离视野（不需要刷新）的区块,并刷新区块
        foreach (var block in refreshingMapBlocks)
        {
            if (Vector2.Distance(block.leftDownPosition, playerMapPos()) > refreshArea)//该区块位置已经超出设置的刷新范围
            {
                block.ExitRefresh();
                removeRecord.Add(block);
            }
            else
            {
                block.Refresh(GameMgr.Get<IGameTimeManager>().GetNow());//刷新区块
            }
        }

        foreach(var reblock in removeRecord)//从刷新队列中移除不在刷新区域中的区块
        {
            refreshingMapBlocks.Remove(reblock);
        }
        removeRecord.Clear();
        #endregion

        mapBlocks[playerPos2MapBlockPos()].ChectInteractiveItem(player);//检测玩家所在区块是否存在进入交互范围的可交互物体

        SetPlayerTemperature();//设置玩家感受到的环境温度

        ShowBlockArea();//在Scene中显示区块的大小
    }

    /// <summary>
    /// 创建新的区块,并初始化
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
        mapBlocks.Add(mapBlockPos, block.GetComponent<MapBlock>());//添加到记录
        block.GetComponent<MapBlock>().Init(instantCreate);//初始化
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
        foreach (var block in mapBlocks.Values)//绘制所有区块的范围
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
        foreach(var block in refreshingMapBlocks)//绘制刷新区块的范围
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
    /// <summary>
    /// 设置玩家感受到的环境温度
    /// </summary>
    private void SetPlayerTemperature()
    {
        Vector2Int playerPosInBlock = playerMapPos()-playerPos2MapBlockPos();//将玩家的整数坐标转换为在区块中相对于左下角的坐标

        //防止数组越界，进行一次四舍五入
        playerPosInBlock.x = Mathf.Max(0, Mathf.Min(playerPosInBlock.x, blockSize * 2 - 1));
        playerPosInBlock.y = Mathf.Max(0, Mathf.Min(playerPosInBlock.y, blockSize * 2 - 1));

        try//如果由于其他问题出错直接跳过，等下次再更新即可，并非关键问题
        {
            player.environmentTemp = mapBlocks[playerPos2MapBlockPos()].environmentTemperature[playerPosInBlock.y, playerPosInBlock.x];
        }
        catch (Exception)
        {
            Debug.LogError($"玩家在区块{playerPos2MapBlockPos()}中不存在位置{new Vector2( playerPosInBlock.x,playerPosInBlock.y)}");
            throw;
        }
        
    }
    /// <summary>
    /// 玩家位置在区块中所在格数
    /// </summary>
    /// <returns></returns>
    private Vector2Int playerMapPos()
    { 
        int x=0,y=0;
        x = (int)((Mathf.Abs(player.transform.position.x) + 0.5f) * Mathf.Sign(player.transform.position.x));
        y = (int)((Mathf.Abs(player.transform.position.y) + 0.5f) * Mathf.Sign(player.transform.position.y));
        return new Vector2Int(x, y);
    }

    //玩家所在区块的左下角位置
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
