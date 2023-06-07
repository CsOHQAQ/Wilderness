using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackPackUI : PackBase
{
    private int curSelect=0;
    private float itemSize = 65;
    private Image selectImg;
    
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        Debug.Log($"#Test 测试{Get<RectTransform>("Panel")}" ); 
        Get<RectTransform>("Panel").sizeDelta=new Vector2(  cargo[0].MaxBattery * (itemSize + Get<HorizontalLayoutGroup>("ItemList").spacing) + Get<HorizontalLayoutGroup>("ItemList").spacing,itemSize+2* Get<HorizontalLayoutGroup>("ItemList").spacing);
        selectImg = Get<Image>("SelectImg");
        selectImg.rectTransform.localPosition = seleceImgPos();
    }
    private void Update()
    {
        while (curSelect >= cargo[0].itemPiles.Count || cargo[0].itemPiles[curSelect] == null)
        {
            if (curSelect == 0)
                break;
            curSelect--;
        }
        if (InputManager.Instance.GetButtonDown(InputEnum.Switch))
        {
            if (curSelect >= cargo[0].itemPiles.Count-1)
            {
                Debug.Log("#Player背包指向0");
                curSelect = 0;
            }
                
            else
            {
                Debug.Log($"#Player背包指向{curSelect}");
                curSelect++;
            }
        }

        selectImg.rectTransform.localPosition = seleceImgPos();

    }

    private Vector2 seleceImgPos()
    {
        float interval = itemSize + Get<HorizontalLayoutGroup>("ItemList").spacing;

        Vector2 pos = new Vector2((-(cargo[0].MaxBattery-1)/2+curSelect)*interval,interval);

        return pos;
    }

}
