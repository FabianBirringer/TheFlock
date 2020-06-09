using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camHandler : MonoBehaviour
{

    public Camera cam1;
    public Camera cam2;
    private GameObject t;

    private int selectedCam = 1;
    // Start is called before the first frame update
    void Start()
    {
        cam1.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("c"))
        {
            if (selectedCam == 1)
            {
                cam2.enabled = true;
                cam1.enabled = false;
                selectedCam = 2;
            }
            else
            {
                cam1.enabled = true;
                cam2.enabled = false;
                selectedCam = 1;
            }
        }
    }

    public void setTargetToCams(GameObject go)
    {
        cam2.GetComponent<attachToBoid>().setTarget(go); 
        cam1.GetComponent<boidtracer>().setTarget(go);
    }
    
}
