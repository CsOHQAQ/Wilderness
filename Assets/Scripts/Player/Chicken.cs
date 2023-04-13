using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class Chicken : MonoBehaviour
{
    private float rotationSpeed=100;
    private float totalAngle = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalAngle = this.transform.rotation.eulerAngles.y+ rotationSpeed * Time.deltaTime;
        this.transform.rotation = Quaternion.Euler(new Vector3(0, totalAngle, 0));
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Player")
        {
            Debug.Log("Win");
            ProcedureManager.Instance.ChangeTo("SelectProcedure");
        }
    }
}
