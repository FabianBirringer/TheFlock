using System;
using System.Collections;
using System.Collections.Generic;

using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

public class BoidBehavior : MonoBehaviour
{
    private ArrayList boids = new ArrayList();
    
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider Other)
    {
        GameObject go = Other.gameObject;
        if (go.CompareTag("boid")&& go.GetInstanceID()!=GetInstanceID())
        {
            if (boids.Contains(go))
            {
                return;
            }
            
            boids.Add(go);
            Debug.Log(boids.Count);
            
        }
    }
    private void OnTriggerExit(Collider Other)
    {
        GameObject go = Other.gameObject;
        if (go.CompareTag("boid") && go.GetInstanceID()!=GetInstanceID())
        {
            if (!boids.Contains(go))
            {
                return;
            }
            boids.Remove(go);
            Debug.Log(boids.Count);
        }
    }
}
