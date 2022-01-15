using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraGecisKontrol : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            CameraControl.ActiveCameraId = (CameraControl.ActiveCameraId + 1) % CameraControl.TotalCamera;
        }
    }
}