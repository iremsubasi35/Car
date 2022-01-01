using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Axel
{
    Front,
    Rear
}
[Serializable]
public struct Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}
public class carcontrol : MonoBehaviour
{
    [SerializeField]
    private float MaximumHizlanma = 200f;
    [SerializeField]
    private float DonusHassasiyeti = 1f;
    [SerializeField]
    private float MaximumDonusAcisi = 45f;
    [SerializeField]
    private List<Wheel> wheels;
    [SerializeField]
    private float inputX, inputY;

    public Vector3 centerOfMass;


    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
    }
    private void LateUpdate()
    {
        Move();
        Turn();
    }

    void Update()
    {
        TekerlerinDonusu();
        HareketYonu();
    }
    void HareketYonu()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }
    private void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.collider.motorTorque = inputY * MaximumHizlanma * 500 * Time.deltaTime;

        }

    }
    void Turn()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel==Axel.Front)
            {
                var _streerAngle = inputX * DonusHassasiyeti * MaximumDonusAcisi;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, _streerAngle, .1f);
            }
           

        }
    }
    void TekerlerinDonusu()
    {
        foreach (var wheel in wheels)
        {
            Quaternion _rot;
            Vector3 _pos;
            wheel.collider.GetWorldPose(out  _pos, out _rot);
            wheel.model.transform.position = _pos;
            wheel.model.transform.rotation = _rot;
        }

    }
    void FrenYap()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.collider.brakeTorque = 1000;
            }

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.collider.brakeTorque = 0;
            }
        }
    }
}
