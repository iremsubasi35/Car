using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform TargetPosition;

    private Transform mainCamera;


    private void OnEnable()
    {
        mainCamera = Camera.main.transform;
        mainCamera.position = TargetPosition.position;
        mainCamera.rotation = TargetPosition.rotation;
        
        mainCamera.parent = transform;
    }

    private void OnDestroy()
    {
        mainCamera.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
