using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;


public class Boidspawn : MonoBehaviour
{ 

    public GameObject boid;
    public Transform spawnPoint;
    public int anzahl;
    public GameObject camHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i<anzahl; i++)
        {
         
            int r1= Random.Range(-(int)Mathf.Sqrt(anzahl) ,(int)Mathf.Sqrt(anzahl));
            int r2 = Random.Range(-anzahl/2, anzahl/2);
            int r3 = Random.Range(-anzahl/2, anzahl/2);
            Vector3 pos = spawnPoint.position + Vector3.up * r1 + Vector3.forward * r2 + Vector3.left * r3;
            float y = 180;
            Quaternion rot = Quaternion.Euler(0,y,0);
            
            GameObject target = Instantiate(boid, pos, rot);
            
            if (i == 0)
            {
                targetToHandler(target);
            }
            



        }    
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void targetToHandler(GameObject go)
    {
        camHandler.GetComponent<camHandler>().setTargetToCams(go);
    }
}
