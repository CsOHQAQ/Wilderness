using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using QxFramework.Core;
using System.Diagnostics;
public class TestSetTilemap : MonoBehaviour
{
    public TileBase tile;
    public Tilemap tilemap;
    private int blockSize = 20;
    private Randomer rand=new Randomer();
    private Stopwatch sw = new Stopwatch();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sw.Start();
            /*
            for(int i = 0; i <= blockSize * 2; i++)
            {
                for(int j = 0; j <= blockSize * 2; j++)
                {
                    //if (rand.nextFloat()>0.5f)
                        tilemap.SetTile(new Vector3Int(j, i, 0), tile);
                }
            }
            */
            StartCoroutine("IESetTile");
            sw.Stop();
            UnityEngine.Debug.Log($"生成用时{sw.ElapsedMilliseconds}");
            sw.Reset();
        }
    }

    IEnumerator IESetTile()
    {
        for (int i = 0; i <= blockSize * 2; i++)
        {
            for (int j = 0; j <= blockSize * 2; j++)
            {
                //if (rand.nextFloat()>0.5f)
                tilemap.SetTile(new Vector3Int(j, i, 0), tile);
                yield return 0;
            }
        }
    }
}
