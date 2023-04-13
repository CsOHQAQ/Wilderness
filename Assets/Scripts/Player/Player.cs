using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody body;
    public bool canControl;
    private void Start()
    {
        body = this.GetComponent<Rigidbody>();
        canControl = true;
    }

    private void Update()
    {
        body.velocity = Vector3.zero;
        if (canControl)
        {
            
            if (Input.GetKey(KeyCode.D))
            {
                
                body.velocity = new Vector3(8, 0, 0);

            }
            if (Input.GetKey(KeyCode.A))
                {
                Debug.Log("left"); 
                    body.velocity = new Vector3(-8, 0, 0);
                }

            
        }        
    }
}
