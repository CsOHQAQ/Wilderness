using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using UnityEngine.UI;
public class RenewableObj : InteractiveObj
{
     public GameDateTime lastVisitTime;
    UIBase interactUI;

    public override void Init(MapBlock block , Object loadInstance=null)
    {
        block = mapBlock;
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    public override void Interact(PlayerBase player)
    {
    }

    public override void Refresh(GameDateTime current)
    {
        lastVisitTime= current;

        
    }
    private void Update()
    {
        if (interactUI != null)
        {
            interactUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, sprite.size.y / 2 + 1));
        }
    }

    public override void OnDestory()
    {
        LeaveInteractRange(); 
    }
    public override void EnterInteractRange()
    {
        interactUI = UIManager.Instance.Open("InteractUI");
        interactUI.transform.position = Camera.main.WorldToScreenPoint(transform.position+new Vector3(0,sprite.size.y/2+2));
    }
    public override void LeaveInteractRange()
    {
        UIManager.Instance.Close(interactUI);
        interactUI = null;
    }
}
