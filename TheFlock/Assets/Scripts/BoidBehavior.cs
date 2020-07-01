using System;
using System.Collections;
using System.Collections.Generic;

using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidBehavior : MonoBehaviour
{
    
    public int id;
    private ArrayList neighborBoids = new ArrayList();
    private ArrayList neighborBehaves= new ArrayList();
    public Rigidbody rb;
    private float radius=30;
    private ArrayList lastAvoidedBoids = new ArrayList();
    private ArrayList lastAvoidingVectors= new ArrayList();
    public Vector3 Velocity;
    private Boidspawn bs;
    private Vector3 avgCenter=new Vector3(0,0,0);
    private Vector3 avgVelocity;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        var localScale = transform.localScale;
        Velocity = new Vector3(5,5,5);
    }

    // Update is called once per frame
    void Update()
    {
        
        
      /*  Vector3 forward = transform.forward;
        Vector3 pos = transform.position;
        
        Vector3 centerDirection = getCenterDirection(pos).normalized/3;

        Vector3 avoidingDirection = getAvoidingDirection(pos);

        Vector3 allignmentDirection = getAlignmentDirection();
        
        transform.forward = forward+((avoidingDirection+centerDirection+allignmentDirection)/3)/2;
        Velocity = (forward + (avoidingDirection+centerDirection+allignmentDirection)/3)/2;
        
        
        */

      Vector3 next = Vector3.ClampMagnitude(calcControll(),40);
      
      Velocity = next; 
      transform.forward = Velocity;
      rb.velocity = Velocity;


          //rb.position += transform.forward;
       // Debug.Log("centerMagnitude: " + centerDirection.magnitude + "avoidingMagnitude: " + avoidingDirection.magnitude + "allignmentMagnitude: " + allignmentDirection.magnitude);

       
       // rb.velocity = Velocity;
       // rb.position += transform.forward;


    }
    
    private Vector3 getCenterDirection(Vector3 pos)
    {
        Vector3 center=new Vector3(0,0,0);
        int c = 0;
        foreach (GameObject boid in neighborBoids)
        {
            
            center += boid.transform.position-pos;
            c++;
        }

        if (c != 0)
        {
            center = (center/c);
        }
        
        return center;
    }

    private Vector3 getAvoidingDirection(Vector3 pos)
    {
        Vector3 avoidingDirection = new Vector3(0,0,0);
        int c = 0;
        lastAvoidedBoids.Clear();
        lastAvoidingVectors.Clear();
        
        foreach (GameObject boid in neighborBoids)
        {
            Vector3 diff = transform.position-boid.transform.position;
            float distance = diff.magnitude;
            float prop = 0;
            
            if (distance < 10 )
            {
                prop = 1 / distance;
                c++;
                avoidingDirection += (diff)*prop;
                lastAvoidedBoids.Add(boid);
                lastAvoidingVectors.Add(diff);
            }
        }
        if (c==0)
        {
            return avoidingDirection;
        }
        
        return avoidingDirection/c;
    }

    private Vector3 getAlignmentDirection()
    {
        Vector3 allignmentDirection = new Vector3(0,0,0);
        int c = 1;
        /*foreach (BoidBehavior bh in neighborBehaves)
        {
            Vector3 otherDirection = bh.Velocity;
            allignmentDirection += otherDirection;
            c++;
        }*/

        foreach (GameObject boid in neighborBoids)
        {
            allignmentDirection += boid.transform.forward;
            c++;
        }

        allignmentDirection = allignmentDirection /c;

        return allignmentDirection;
    }
    
    //fügt boids in der Umgebung einer Liste hinzu
    private void OnTriggerEnter(Collider Other)
    {
        GameObject go = Other.gameObject;
        if (go.CompareTag("boid")&& go.GetInstanceID()!=GetInstanceID()&& !go.CompareTag("collider"))
        {
            if (neighborBoids.Contains(go))
            {
                return;
            }

            
            
            

            neighborBehaves.Add(go.GetComponent<BoidBehavior>() as BoidBehavior);
            neighborBoids.Add(go);
            //Debug.Log(go.tag+"entered radius, count:"+boids.Count);

        }
    }
    //entfernt boids aus der Liste sobald dieser sich aus dem radius bewegt
    private void OnTriggerExit(Collider Other)
    {
        GameObject go = Other.gameObject;
        if (go.CompareTag("boid") && go.GetInstanceID()!=GetInstanceID())
        {
            if (!neighborBoids.Contains(go))
            {
                return;
            }
            

            neighborBoids.Remove(go);
            //Debug.Log(go.tag+"left radius, count: "+boids.Count);
        }
    }

    public void setID(int i)
    {
        id = i;
    }

    public int getID()
    {
        return id;
    }

    public void fillMatrice(float[,] m)
    {
        foreach (BoidBehavior n in neighborBehaves)
        {
            m[id, n.getID()] = bumpF(transform.position, n.getPos());
           // m[id, n.getID()] = 1;
           //m[id, n.getID()] = (transform.position - n.getPos()).magnitude;
        }
    }

    public Vector3 getPos()
    {
        return transform.position;
    }

    public float bumpF(Vector3 x, Vector3 y)
    {
        float z = 0;
        float h = 0.5f;
        z = onorm(x-y)/radius;
        if (z <= h)
        {
            return 1;
        }
        if (z > h && z < 1)
        {
            return 0.5f * (1 + Mathf.Cos(Mathf.PI * ((z - h) / (1 - h))));
        }

        if (z >= 1)
        {
            return 0;
        }

        return -1;

    }

    public float onorm(Vector3 z)
    {
        float f = 0;
        float e = 0.5f;
        return 1 / e * (Mathf.Sqrt(1 + e * z.sqrMagnitude) - 1);
    }
    
    
    //hehe
    private float dD = 16; //desiredDistance;
    public float getBPotEnergy()
    {
        float e = 0;
        float avg = 0;
        foreach (GameObject boid in neighborBoids)
        {
            Vector3 diff = transform.position - boid.transform.position;
            e += potFunc((diff.magnitude - dD)/10);
            avg += diff.magnitude;
        }

        if (neighborBoids.Count != 0)
        {
            avg = avg / neighborBoids.Count;
        }
        Debug.Log("Potential Energy in boid "+id+" neighbourhood: "+e+" avg Distance to boids: "+avg);
        return e;
    }

    private  float b=5;
    private float a=1;
    public float potFunc(float z)
    { 
        float c = (b - a) / (2 * Mathf.Sqrt(a * b));
        return ((a + b)/2 )* (Mathf.Sqrt(1+((z+c)*(z+c))) - Mathf.Sqrt(1 + (c * c))) + ((a - b) / 2) * z;
    }

    public float actFunc(float z)
    {
        float c = (b - a) / (2 * Mathf.Sqrt(a * b));
        return ((a + b) / 2) * ((z + c) / (Mathf.Sqrt((1 + (z + c) * (z + c))))) + ((a - b) / 2);
    }

    public Vector3 calcControll()
    {
        float c = 0.1f;
        Vector3 v = new Vector3(0, 0, 0);
        foreach (GameObject boid in neighborBoids)
        {
            Vector3 diff =boid.transform.position - transform.position;
            v += actFunc(diff.magnitude - dD) * diff.normalized + c* ((boid.GetComponent<BoidBehavior>().Velocity-avgVelocity)-(Velocity-avgVelocity));
        }
        

        return v; 
    }

    public void setBS(Boidspawn bs)
    {
        this.bs = bs;
    }

    public void setAvgVel(Vector3 v)
    {
        avgVelocity = v;
    }

    public void setAvgCenter(Vector3 c)
    {
        avgCenter = c;
    }

    
}
