using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sıralama : MonoBehaviour
{
    public int AktıfYonSırası = 1;
    SıralamaYonetım sıralama;


    void Start()
    {
        sıralama = GameObject.FindWithTag("OyunKontrol").GetComponent<SıralamaYonetım>();
        sıralama.kendinigonder(gameObject,AktıfYonSırası);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("YonBul"))
        {
            AktıfYonSırası =int.Parse( other.transform.gameObject.name);
            sıralama.SıralamaGuncelle(gameObject,AktıfYonSırası);
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
