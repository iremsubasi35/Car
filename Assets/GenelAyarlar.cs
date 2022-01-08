using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenelAyarlar : MonoBehaviour
{
    public GameObject[] Araclar;
    public GameObject SpawnPoint;
    Transform pivot;
    Transform pozisyon;
    void Start()
    {
        GameObject arabam = Instantiate(Araclar[PlayerPrefs.GetInt("SecilenArac")], SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        pivot = arabam.transform.Find("Pivot");
        pivot = arabam.transform.Find("PozisyonAl");

         GameObject.Find("MainCamera").GetComponent<KameraGecisKontrol>().target[0] = pozisyon;
         GameObject.Find("Main Camera").GetComponent<KameraGecisKontrol>().target[1] = pivot;
    }



}
