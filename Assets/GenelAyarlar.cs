using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenelAyarlar : MonoBehaviour
{
    public GameObject[] Araclar;
    public GameObject SpawnPoint;

    private CameraControl camControl;

    void Start()
    {
        camControl = FindObjectOfType<CameraControl>();
        GameObject arabam = Instantiate(Araclar[PlayerPrefs.GetInt("SecilenArac")], SpawnPoint.transform.position, SpawnPoint.transform.rotation);


        // camControl.target[0] = arabam.transform.Find("PozisyonAl");
        // camControl.target[1] = arabam.transform.Find("Pivot");

        // GameObject.Find("OyunKontrol").GetComponent<KameraGecisKontrol>().kameralar[0] = arabam.transform.Find("Kameralar/OnKaput").gameObject;
        // GameObject.Find("OyunKontrol").GetComponent<KameraGecisKontrol>().kameralar[1] = arabam.transform.Find("Kameralar/Aracici").gameObject;
    }
}