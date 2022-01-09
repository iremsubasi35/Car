using System;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public Text CurrentSpeed;
    public Text GearText;
    public GameObject SpeedoMeter;
    public Image NitroSlider;
    public Text NitroSliderText;
    public GameObject NitroEffect;


    public static UserInterface master;

    private void Start()
    {
        master = this;
    }

    private void OnDestroy()
    {
        master = null;
    }
}