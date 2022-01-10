using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenelAyarlar : MonoBehaviour
{
    public GameObject[] Araclar;
    public GameObject[] YapayZekaSpawnPoint;
    public GameObject[] YapayZekaAraclar;
    public GameObject SpawnPoint;

    private CameraControl camControl;


    void Start()
    {
        camControl = FindObjectOfType<CameraControl>();
        GameObject arabam = Instantiate(Araclar[PlayerPrefs.GetInt("SecilenArac")], SpawnPoint.transform.position, SpawnPoint.transform.rotation);


        GameObject.Find("Main Camera").GetComponent<CameraControl>().target[0] = arabam.transform.Find("PozisyonAl");
        GameObject.Find("Main Camera").GetComponent<CameraControl>().target[1] = arabam.transform.Find("Pivot");

        GameObject.Find("OyunKontrol").GetComponent<KameraGecisKontrol>().kameralar[1] = arabam.transform.Find("Kameralar/OnKaput").gameObject;
        GameObject.Find("OyunKontrol").GetComponent<KameraGecisKontrol>().kameralar[2] = arabam.transform.Find("Kameralar/Aracici").gameObject;
        for (int i = 0; i < 2; i++)  // 2 sözgelimi yazýlmýstýr
        {
            int randomdeger = Random.Range(0, YapayZekaAraclar.Length - 1);
            Instantiate(YapayZekaAraclar[randomdeger], YapayZekaSpawnPoint[i].transform.position, YapayZekaSpawnPoint[i].transform.rotation);
        }
    }
}