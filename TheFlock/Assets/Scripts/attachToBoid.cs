using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attachToBoid : MonoBehaviour
{
    private GameObject target;

    private Boolean targetSet = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetSet)
        {
            transform.position = target.transform.position+new Vector3(0,10,0);
            float h = 2.0f * Input.GetAxis("Mouse X");
            float v = -2.0f * Input.GetAxis("Mouse Y");

            transform.Rotate(v, h, 0);
        }
        
    }

    private void LateUpdate()
    {
       
    }

    public void setTarget(GameObject go)
    {
        target = go;
        targetSet = true;
    }
}
