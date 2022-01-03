using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform[] target;
    
  

    // Update is called once per frame
    void Update()
    {
        transform.position = target[0].position;
        transform.LookAt(target[1].position); 
    }
}
