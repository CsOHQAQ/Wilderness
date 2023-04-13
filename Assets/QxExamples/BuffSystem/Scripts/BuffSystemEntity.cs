using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 此为测试buff效果的游戏内GameObject，用于展示修改buff如何对Game Object产生影响
/// </summary>
public class BuffSystemEntity : MonoBehaviour
{
    private float oriSpeed=3;

    public float Speed=>oriSpeed*Mathf.Max((buffManager.dataChanger.MoveSpeedMul._index+1),0);
    public BuffManager buffManager;


    private float count=0;
    private bool isMovingLeft = true;
    private void Awake()
    {
        count = 0;
        buffManager = GetComponent<BuffManager>();
        buffManager.Init();

    }

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.left * Speed;
        if (isMovingLeft)
        {
            count += Time.deltaTime;
            if (count >= 2)
            {
                oriSpeed *= -1;
                isMovingLeft = false;
            }
                
        }
        else
        {
            count -= Time.deltaTime;
            if (count <= 0)
            {
                oriSpeed *= -1;
                isMovingLeft = true;
            }
        }

    }

}
