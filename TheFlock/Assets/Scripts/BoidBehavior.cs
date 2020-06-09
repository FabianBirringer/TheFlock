using System;
using System.Collections;
using System.Collections.Generic;

using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

public class BoidBehavior : MonoBehaviour
{
    private ArrayList boids = new ArrayList();
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider Other)
    {
        GameObject go = Other.gameObject;
        if (go.tag == "boid"&& go.GetInstanceID()!=GetInstanceID() && go.tag!="collider")
        {
            if (boids.Contains(go))
            {
                return;
            }
            boids.Add(go);
        }
    }
    private void OnTriggerExit(Collider Other)
    {
        GameObject go = Other.gameObject;
        if (go.tag == "boid" && go.GetInstanceID()!=GetInstanceID())
        {
            boids.Remove(go);
        }
    }
}
