using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class CameraController : MonoBehaviour
{
    public bool needFollow = false;
    private PlayerBase player;
    public void Init(PlayerBase p)
    {
        player = p;
    }
    public void Update()
    {
        if (ProcedureManager.Instance.Current is GameProcedure)
        {
            transform.position = player.transform.position - new Vector3(0, 0, 50);
        }
        else
        {
            transform.position = new Vector3(0, 0, -50);
        }
    }
}
