﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using QxFramework.Core;
using System;

public class MapBlock:MonoBehaviour
{
    [HideInInspector]
    public Vector2Int Position;

    [HideInInspector]
    public int ID;

    [HideInInspector]
    public Dictionary<Vector2Int,SingleMapBlock> singleBlocks;

    [HideInInspector]
    public GameDateTime lastVisitTime;

    [HideInInspector]
    public int blockSize = 20;

    private Tilemap backgroundTilemap;
    //用于上层地面的tilemap
    private Tilemap decorationTilemap_Ground;
    //用于记录地面上装饰（如花朵）的tilemap
    private Tilemap decorationTilemap_Object;

    //在场景中设置
    public TileBase backgroundTile;
    public TileBase decGroundTile;
    public TileBase decObjTile;

    /// <summary>
    /// 仅在被创建时调用的初始化罢大概
    /// </summary>
    /// <param name="instantCreate">是否需要立即生成</param>
    public void Init(bool instantCreate=false)
    {
        lastVisitTime = GameMgr.Get<IGameTimeManager>().GetNow();

        backgroundTilemap = this.transform.Find("Background").GetComponent<Tilemap>();
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
        
    }

    public void EnterRefresh()
    {

    }

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

    private void SummonInterativeObj()
    {

    }
}

public class SingleMapBlock
{
    public Sprite sprite;
}