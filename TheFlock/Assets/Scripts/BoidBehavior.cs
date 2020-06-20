using System;
using System.Collections;
using System.Collections.Generic;

using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

public class BoidBehavior : MonoBehaviour
{
    private ArrayList boids = new ArrayList();
    public Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.forward;
        Vector3 pos = transform.position;
        
        Vector3 centerDirection = getCenterDirection(pos).normalized/3;

        Vector3 avoidingDirection = getAvoidingDirection(pos);

        Vector3 allignmentDirection = getAlignmentDirection();
        
        transform.forward = (forward+((avoidingDirection+centerDirection+allignmentDirection)).normalized)/2;

        rb.position += transform.forward;
        Debug.Log("centerMagnitude: " + centerDirection.magnitude + "avoidingMagnitude: " + avoidingDirection.magnitude + "allignmentMagnitude: " + allignmentDirection.magnitude);
    }
    
    private Vector3 getCenterDirection(Vector3 pos)
    {
        Vector3 center=new Vector3(0,0,0);
        int c = 0;
        foreach (GameObject boid in boids)
        {
            if (c == 0)
            {
                center = boid.transform.position-pos;
            }
            else
            {
                center += boid.transform.position-pos;
            }
            c++;
        }

        if (c == 0)
        {
            return pos;
        }
        center = (center/c);
        return center;
    }

    private Vector3 getAvoidingDirection(Vector3 pos)
    {
        Vector3 avoidingDirection = new Vector3(0,0,0);
        int c = 0;
        
        foreach (GameObject boid in boids)
        {
            Vector3 otherPos = boid.transform.position;
            float distance = (otherPos - pos).magnitude;
            float prop = 0;
            
            if (distance < 10 && distance >-10)
            {
                prop = 1 / distance;
                c++;
                avoidingDirection += (pos-otherPos)*prop;
            }
        }
        if (c==0)
        {
            return avoidingDirection;
        }
        avoidingDirection = avoidingDirection ;
        
        return avoidingDirection/c;
    }

    private Vector3 getAlignmentDirection()
    {
        Vector3 allignmentDirection = new Vector3(0,0,0);
        int c = 1;
        foreach (GameObject boid in boids)
        {
            Vector3 otherDirection = boid.transform.forward;
            allignmentDirection += otherDirection;
            c++;
        }

        allignmentDirection = allignmentDirection /c;

        return allignmentDirection;
    }
    
    
    
    
    
    

    //fügt boids in der Umgebung einer Liste hinzu
    private void OnTriggerEnter(Collider Other)
    {
        GameObject go = Other.gameObject;
        if (go.CompareTag("boid")&& go.GetInstanceID()!=GetInstanceID()&& go.tag!="collider")
        {
            if (boids.Contains(go))
            {
                return;
            }
            
            boids.Add(go);
            //Debug.Log(boids.Count);
            
        }
    }
    //entfernt boids aus der Liste sobald dieser sich aus dem radius bewegt
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
            //Debug.Log(boids.Count);
        }
    }
}
