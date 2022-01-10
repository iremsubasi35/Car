using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CameraControl : MonoBehaviour
{
    public static int ActiveCameraId = 0;
    public static YapayZekaController playerController;
    public static int TotalCamera => playerController.Target.Length;

    // Update is called once per frame
    void Update()
    {
        if (playerController == null) return;
        
        transform.position = playerController.Target[ActiveCameraId].position;
        transform.rotation = playerController.Target[ActiveCameraId].rotation;
    }

    public static void ChangeCamera()
    {
        
    }
}
