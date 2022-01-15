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

    private void Start()
    {
        activateCamera();
    }

    void activateCamera()
    {
        foreach (var cam in playerController.Target)
            cam.gameObject.SetActive(false);
        
        playerController.Target[ActiveCameraId].gameObject.SetActive(true);
    }

    public void NextCamera()
    {
        if (!playerController.IsCurrentPlayer) return;
        
        ActiveCameraId = (ActiveCameraId + 1) % TotalCamera;
        activateCamera();
    }

}
