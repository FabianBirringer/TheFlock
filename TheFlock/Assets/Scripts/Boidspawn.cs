using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;



  public class Boidspawn  : MonoBehaviour
  {
      private static float[,] _matrix;
      private int[,] matrix;

    public GameObject boid;
    public Transform spawnPoint;
    public int anzahl;
    public GameObject camHandler;
    private ArrayList allboids = new ArrayList();
    private static ArrayList _boidbehaves =new ArrayList();
    private int count;
    public static Vector3 AllCenter;
    public static Vector3 AllVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _matrix=new float[anzahl,anzahl];
        for (int i = 0; i<anzahl; i++)
        {
            for (int j = 0; j < anzahl; j++)
            {
                _matrix[i, j] = 0;
            }

            float s = 0.5f ;
            float r1= Random.Range(-anzahl*s ,anzahl*s);
            float r2 = Random.Range(-anzahl*s ,anzahl*s);
            float r3 = Random.Range(-anzahl*s ,anzahl*s);
            Vector3 pos = spawnPoint.position + Vector3.up * r1 + Vector3.forward * r2+ Vector3.left * r3;
            float y = 20;
            Quaternion rot = Quaternion.Euler(0,y*i,0);
            
            GameObject target = Instantiate(boid, pos, rot);
            allboids.Add(target);
            BoidBehavior bh = target.GetComponent<BoidBehavior>();
            _boidbehaves.Add(bh as BoidBehavior);
            bh.setID(i);
            bh.setBS(this);
            
            if (i == 0)
            {
                targetToHandler(target);
            }
        }
        
        foreach (GameObject boid in allboids)
        {
            AllCenter += boid.transform.position;
            AllVelocity += boid.transform.forward;
        }
        AllCenter /= allboids.Count;
        AllVelocity /= allboids.Count;
        
    }

    

    // Update is called once per frame
    void Update()
    {
        count++;
        if (count > 1000)
        {
            count = 0;
            //calcDeviationEnergy();
            Debug.Log("Structurelle Energie: "+getStructEnergy()+" Potentielle Energie: "+getPotEnergy()+" relative kinetische Energie: "+ getRelKinEnergy());
        }
        
       AllCenter = new Vector3(0,0,0);
       AllVelocity = new Vector3(0,0,0);

       foreach (GameObject boid in allboids)
       {
           AllCenter += boid.transform.position;
           AllVelocity += boid.transform.forward;
       }
       AllCenter /= allboids.Count;
       AllVelocity /= allboids.Count;
       
       
    }
    
    
    private void targetToHandler(GameObject go)
    {
        camHandler.GetComponent<camHandler>().setTargetToCams(go);
    }

    static void calcDeviationEnergy()
    {
        foreach (BoidBehavior b in _boidbehaves)
        {
            b.fillMatrice(_matrix);
        }

        string line = "";
        for (int i = 0; i < _boidbehaves.Count; i++)
        {
            //string line = "";
            for (int j = 0; j < _boidbehaves.Count; j++)
            {
                line+=("|" + _matrix[i, j] + "|");
            }

            line += Environment.NewLine;
            //Debug.Log(line);
        }
        Debug.Log(line);
    }

    private float getPotEnergy()
    {
        float pEnergy = 0;
        foreach (BoidBehavior bh in _boidbehaves)
        {
            pEnergy += bh.getBPotEnergy();
        }
        return pEnergy;
    }

    private float getRelKinEnergy()
    {
        float e = 0;
        foreach (BoidBehavior bh in _boidbehaves)
        {
            e += (bh.Velocity - AllVelocity).magnitude*( bh.Velocity - AllVelocity).magnitude;
        }

        return e / 2;
    }

    private float getStructEnergy()
    {
        return getPotEnergy() + getRelKinEnergy();
    }

    
}
