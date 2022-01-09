using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CameraControl : MonoBehaviour
{
    public Transform[] target;

    public static YapayZekaController playerController;

    // Update is called once per frame
    void Update()
    {
        if (playerController == null) return;

        transform.position = playerController.Target[0].position;
        transform.rotation = playerController.Target[0].rotation;
    }
}
