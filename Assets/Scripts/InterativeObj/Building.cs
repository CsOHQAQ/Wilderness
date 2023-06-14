using System.Collections;
using System.Collections.Generic;
using QxFramework.Core;
using UnityEngine;

public class Building : InteractiveObj
{
    public string buildingName = "";
    public Dictionary<Item, int> needItems = new Dictionary<Item, int>();
    public GameDateTime needTime = new GameDateTime();

    public UIBase interactUI;
    public float buildingProgress;

    public override void Init(MapBlock block, Object loadInstance = null)
    {
        mapBlock = block;
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();        
    }
    public override void Interact(PlayerBase player)
    {

    }
    public override void Refresh(GameDateTime jumpTime)
    {

    }
    private void Update()
    {
        if (interactUI != null && interactUI.isActiveAndEnabled)
        {
            interactUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, sprite.size.y / 2 + 1));
        }
    }
    public virtual void OnFinishBuild()
    {

    }
    public override void OnDestory()
    {
        LeaveInteractRange();
    }
    public override void EnterInteractRange()
    {
        interactUI = UIManager.Instance.Open("InteractUI");
        interactUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, sprite.size.y / 2 + 5));
    }
    public override void LeaveInteractRange()
    {
        
    }
}
