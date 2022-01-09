using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenelAyarlar : MonoBehaviour
{
    public GameObject[] Araclar;
    public GameObject SpawnPoint;
    
    void Start()
    {
        GameObject arabam = Instantiate(Araclar[PlayerPrefs.GetInt("SecilenArac")], SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        

         GameObject.Find("MainCamera").GetComponent<CameraControl>().target[0] = arabam.transform.Find("PozisyonAl");
         GameObject.Find("Main Camera").GetComponent<CameraControl>().target[1] = arabam.transform.Find("Pivot");

        GameObject.Find("OyunKontrol").GetComponent<KameraGecisKontrol>().kameralar[1] = arabam.transform.Find("Kameralar/OnKaput").gameObject;
        GameObject.Find("OyunKontrol").GetComponent<KameraGecisKontrol>().kameralar[1] = arabam.transform.Find("Kameralar/Aracici").gameObject;
    }



}
