using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class boidtracer : MonoBehaviour
{
    private GameObject target;

    private Boolean targetSet = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (targetSet)
        {
            transform.LookAt(target.transform);
        }
        ;
    }

    public void setTarget(GameObject go)
    {
        target = go;
        targetSet = true;
    }

    
}
