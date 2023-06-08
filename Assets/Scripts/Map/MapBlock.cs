using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using QxFramework.Core;
using System;

public class MapBlock:MonoBehaviour
{
    [HideInInspector]
    public Vector2Int leftDownPosition=new Vector2Int();

    [HideInInspector]
    public int ID;

    [HideInInspector]
    public GameDateTime lastVisitTime=new GameDateTime();
    private GameDateTime lastSummonTime = new GameDateTime();
    private int intObjNumLimit = 12;

    [HideInInspector]
   //blockSize表示区块长的一半
    public int blockSize = 20;

    public float[,] environmentTemperature;
    private List<InteractiveObj> intObjList=new List<InteractiveObj>();

    //用于上层地面的tilemap
    private Tilemap decorationTilemap_Ground;
    //用于记录地面上装饰（如花朵）的tilemap
    private Tilemap decorationTilemap_Object;

    //在场景中设置
    public TileBase backgroundTile;
    public TileBase decGroundTile;
    public TileBase decObjTile;

    public InteractiveObj curInteractableObj = null;

    /// <summary>
    /// 仅在被创建时调用的初始化罢大概
    /// </summary>
    /// <param name="instantCreate">是否需要立即生成</param>
    public void Init(bool instantCreate=false)
    {
        lastVisitTime = GameMgr.Get<IGameTimeManager>().GetNow();
        lastSummonTime = lastVisitTime;
        SummonInterativeObj();
        decorationTilemap_Ground = this.transform.Find("Decoration_Ground").GetComponent<Tilemap>();
        decorationTilemap_Object = this.transform.Find("Decoration_Obj").GetComponent<Tilemap>();

        if (!instantCreate)
        {
            List<List<int>> decMap = GenerateRandDecoration(0.35f);
            StartCoroutine(SetAreaTiles(decorationTilemap_Ground.GetComponent<Tilemap>(), decGroundTile, decMap));
        }
        else
        {
            List<List<int>> decMap = GenerateRandDecoration(0.35f);
            for (int i = 0; i < decMap.Count; i++)
            {
                for (int j = 0; j < decMap[i].Count; j++)
                {
                    if (decMap[i][j] != 0)
                        decorationTilemap_Ground.SetTile(new Vector3Int(j, i, 0), decGroundTile);
                }
            }

        }
        environmentTemperature = new float[blockSize * 2,blockSize * 2];
        for(int i = 0; i < blockSize * 2; i++)
        {
            for (int j = 0; j < blockSize * 2; j++)
                environmentTemperature[i, j] = 0.5f;
        }
    }
    /// <summary>
    /// 区块进入刷新队列
    /// </summary>
    /// <param name="Current"></param>
    public void EnterRefresh(GameDateTime Current)
    {

        lastVisitTime = Current;
    }

    /// <summary>
    /// 区块刷新
    /// </summary>
    /// <param name="Current"></param>
    public void Refresh(GameDateTime Current)
    {
        //检测是否有空的交互物体
        List<InteractiveObj> removeList = new List<InteractiveObj>();
        foreach (var obj in intObjList)
        {
            if (obj == null)
            {
                removeList.Add(obj);
            }
        }
        foreach (var obj in removeList)
        {
            intObjList.Remove(obj);
        }

        //刷新所有的交互物体
        foreach (var obj in intObjList)
        {
            obj.Refresh(Current);
        }
        lastVisitTime = Current;

        //如果太久未生成可交互物体，就生成一批
        if (intObjList.Count < intObjNumLimit && (Current-lastSummonTime).TotalMinutes/60>24)
        {
            SummonInterativeObj();
            lastSummonTime = Current;
        }
    }

    /// <summary>
    /// 区块退出刷新队列
    /// </summary>
    public void ExitRefresh()
    {

    }

    /// <summary>
    /// 使用协程实现设置一片区块，保证性能
    /// </summary>
    /// <param name="tilemap"></param>
    /// <param name="tile"></param>
    /// <param name="mapData"></param>
    /// <returns></returns>
    private IEnumerator SetAreaTiles(Tilemap tilemap, TileBase tile, List<List<int>> mapData = null)
    {
        if (mapData != null)
        {
            for (int i = 0; i < mapData.Count; i++)
            {
                for (int j = 0; j < mapData[i].Count; j++)
                {
                    if (mapData[i][j] != 0)
                        tilemap.SetTile(new Vector3Int(j, i, 0), tile);
                }
                yield return 0;
            }
        }
        else
        {
            Debug.Log("#Map");
            for (int i = 0; i < blockSize * 2; i++)
            {
                for (int j = 0; j < blockSize * 2; j++)
                {
                    tilemap.SetTile(new Vector3Int(j, i, 0), tile);
                }
                yield return 0;
            }
        }
    }

    /// <summary>
    /// 按区块大小（blockSize）随机生成装饰的算法
    /// </summary>
    /// <param name="decProbabiliaty">每一格生成装饰的概率</param>
    /// <returns></returns>
    private List<List<int>> GenerateRandDecoration(float decProbabiliaty)
    {
        List<List<int>> randMap = new List<List<int>>();
        Randomer randomer = new Randomer((ulong)System.DateTime.Now.GetHashCode());

        //先随机生成一定的Decoration
        for (int y = 0; y < blockSize * 2; y++)
        {
            randMap.Add(new List<int>());

            for (int x = 0; x < blockSize * 2; x++)
            {
                float proba = randomer.nextFloat();
                //Debug.Log($"位置{x},{y}的概率值为{proba}");
                randMap[y].Add(proba < decProbabiliaty ? 1 : 0);//1为decoration，0为没有
                if (x == 0 || x == blockSize * 2 - 1 || y == 0 || y == blockSize * 2 - 1)
                {
                    randMap[y][x] = 0;
                }
            }
        }
        //将散落的dec点链接起来
        for (int repeatTimes = 0; repeatTimes < 2; repeatTimes++)
        {
            for (int y = 1; y < blockSize * 2 - 1; y++)
            {
                for (int x = 1; x < blockSize * 2 - 1; x++)
                {
                    Func<int> getDecNeighboor = () =>//获得周围一圈被标记为dec的格子数量
                    {
                        int ans = 0;
                        for (int i = -1; i <= 1; i++)//i和j为搜索的范围
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (i == 0 && j == 0)
                                    continue;
                                if (randMap[y + i][x + j] == 1)
                                    ans++;
                            }
                        }
                        return ans;
                    };
                    if (getDecNeighboor.Invoke() >= 4)
                    {
                        //Debug.Log($"({x},{y})障碍物够");
                        randMap[y][x] = 1;
                    }
                    else
                    {
                        randMap[y][x] = 0;
                    }
                }
            }
        }

        return randMap;
    }

    /// <summary>
    /// 在场上生成可交互物体
    /// </summary>
    private void SummonInterativeObj()
    {
        Randomer rand = new Randomer();
        
        int objPointNum = (int)Mathf.Abs( rand.nextNormal(2,0.4f));
        Debug.Log($"#Map生成资源点{objPointNum}个");
        int objItemNum=3;//这个是一个资源点会有几个资源的基数
        float objPointRange = 5f;
        float objPointInterval = 15f;

        for(int pointId = 1; pointId <= objPointNum; pointId++)
        {
            Vector2 pointPos = new Vector2(rand.nextFloat() * (blockSize * 2 - 2 * objPointRange) + objPointRange + leftDownPosition.x, rand.nextFloat() * (blockSize * 2 - 2 * objPointRange) + objPointRange + leftDownPosition.y);
            bool flag = true;
            int runTimes = 0;
            while (flag && runTimes < 20)
            {
                runTimes++;
                foreach(var otherPoint in intObjList)
                {
                    if (Vector2.Distance(otherPoint.transform.position, pointPos) < objPointInterval)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    break;
                }
                pointPos = new Vector2(rand.nextFloat() * blockSize * 2 + leftDownPosition.x, rand.nextFloat() * blockSize * 2 + leftDownPosition.y);
            }
            if (runTimes == 20)
                Debug.LogError($"#Map区块{leftDownPosition}未能在满足间隔的情况下生成随机物体");
            objItemNum = (int)Mathf.Abs( rand.nextNormal(objItemNum,0.5f)) + 1;
            Debug.Log($"#Map资源点内有{objItemNum}个");

            for (int itemId = 1; itemId <= objItemNum; itemId++)
            {
                float range = rand.nextFloat()* objPointRange, angle=rand.nextFloat()*2*Mathf.PI;//生成随机半径和角度
                InteractiveObj intObj = ResourceManager.Instance.Instantiate("Prefabs/InteractiveObj/Plant/WoodTree").GetComponent<InteractiveObj>();
                intObj.transform.SetParent(this.transform);
                intObj.Init();
                intObj.transform.position = new Vector2(Mathf.Cos(angle),Mathf.Sin(angle))*range+pointPos;
                intObjList.Add(intObj);
            }

        }
    }

    /// <summary>
    /// 检测player周围是否有可交互物体
    /// </summary>
    /// <param name="player"></param>
    public void ChectInteractiveItem(PlayerBase player)
    {
        //从这里开始做进入交互范围的提示，把果树做出来，再把建筑物整出来，如果不行的话就不做建筑物
        InteractiveObj obj = null;
        float minDis = 114514f;
        foreach (var intObj in intObjList)
        {
            float dis = Vector2.Distance(intObj.transform.position, player.transform.position);
            if (dis < minDis && dis <= player.interactRange)
            {
                obj = intObj;
                minDis = dis;
            }
        }
        if (obj != null)
        {
            Debug.Log($"#Map最近交互物{obj.transform.position},距离{minDis}");
        }
        if (obj == null)
        {
            if (curInteractableObj != null)
            {
                curInteractableObj.LeaveInteractRange();
            }
            curInteractableObj = null;
            return;
        }
        if (obj != null&&curInteractableObj!=obj)
        {
            if(curInteractableObj!=null)
                curInteractableObj.LeaveInteractRange();
            obj.EnterInteractRange();
            curInteractableObj = obj;
        }
        
    }

    /// <summary>
    /// 玩家执行交互
    /// </summary>
    /// <param name="player"></param>
    public void Interact(PlayerBase player)
    {
        
        InteractiveObj obj=null;
        float minDistance = 1000000f;
        foreach(var intObj in intObjList)
        {
            float distance = Vector2.Distance(intObj.transform.position, player.transform.position);
            if (distance<player.interactRange&&distance<minDistance)
            {
                obj = intObj;
                minDistance = distance;
            }
        }
        if (obj != null)
        {
            obj.Interact(player); 
        }
    }
}