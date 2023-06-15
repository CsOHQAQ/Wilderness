using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackPackUI : PackBase
{
    private int curItemPile=-1;
    private float itemSize = 65;
    private Image selectImg;
    
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        Get<RectTransform>("Panel").sizeDelta=new Vector2(  cargo[0].MaxBattery * (itemSize + Get<HorizontalLayoutGroup>("ItemList").spacing) + Get<HorizontalLayoutGroup>("ItemList").spacing,itemSize+2* Get<HorizontalLayoutGroup>("ItemList").spacing);
        selectImg = Get<Image>("SelectImg");
        selectImg.rectTransform.localPosition = seleceImgPos();
    }
    private void Update()
    {
        while ((curItemPile>-1)&&(curItemPile >= cargo[0].itemPiles.Count || cargo[0].itemPiles[curItemPile] == null))
        {
            if (curItemPile == -1)
                break;
            if (cargo[0].itemPiles.Count > 0)
            {
                curItemPile = cargo[0].itemPiles.Count - 1;
            }
            else
            {
                curItemPile = -1;
            }
        }
        if (InputManager.Instance.GetButtonDown(InputEnum.Switch))
        {
            if (cargo[0].itemPiles.Count <= 0)
            {
                curItemPile = -1;                
            }
            else
            {
                if (curItemPile >= cargo[0].itemPiles.Count - 1)
                {
                    Debug.Log("#Player背包指向0");
                    curItemPile = 0;
                }

                else
                {
                    Debug.Log($"#Player背包指向{curItemPile}");
                    curItemPile++;
                }
            }
            
        }

        selectImg.rectTransform.localPosition = seleceImgPos();

    }

    private Vector2 seleceImgPos()
    {
        float interval = itemSize + Get<HorizontalLayoutGroup>("ItemList").spacing;
        Vector2 pos = new Vector2();
        if (curItemPile == -1)
        {
            pos = new Vector2((-(cargo[0].MaxBattery - 1) / 2 ) * interval, interval);
        }
        else
        {
            pos = new Vector2((-(cargo[0].MaxBattery - 1) / 2 + cargo[0].itemPiles[curItemPile].CurrentPosID) * interval, interval);
        }

        return pos;
    }
    public ItemPile GetCurrentItem()
    {
        return cargo[0].itemPiles[curItemPile];
    }
}
