using System.Collections;
using QxFramework.Core;
using UnityEngine;
/// <summary>
/// 这个是掉落物的类
/// </summary>
public class DropItem : MonoBehaviour
{
    public ItemPile itemPile;
    public float maxWaitTime = 4f;//再次拾取所需的时间（现实世界的秒）
    public float playerEnterTime=0;
    private CargoData cargo;//被捡起来的时候添加到哪个背包
    private float moveAngle = 0;
    /// <summary>
    /// 初始化掉落物
    /// </summary>
    /// <param name="items">掉落物对应的物品堆</param>
    /// <param name="playerCargo">其实就是被捡起来的时候添加到哪个背包</param>
    /// <param name="initTime">在生成的时候假设玩家已经等待了多久，用于不是玩家主动丢弃的掉落物（玩家主动想捡起来的）</param>
    public void Init(ItemPile items,CargoData playerCargo,float initTime=0)
    {
        itemPile = items;
        cargo = playerCargo;
        playerEnterTime = initTime;
        GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.Load<Sprite>("Texture/Property/" + itemPile.item.ItemImg);//设置掉落物的图片
    }

    private void Update()
    {
        transform.position += new Vector3(0, Mathf.Sin(moveAngle))*0.1f*Time.deltaTime;//尝试实现一个简易的浮动效果
        moveAngle += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerEnterTime += Time.deltaTime;
            if (playerEnterTime > maxWaitTime)
            {
                if (GameMgr.Get<IItemManager>().AddItem(itemPile.item.ItemID, 1, cargo))
                {
                    itemPile.CurrentPile--;
                    if (itemPile.CurrentPile <= 0)
                        GameObject.Destroy(this.gameObject);
                }
            }
        }
    }

}
